using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Features.Commands.RefreshToken;

public class RefreshTokensCommandHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokensCommandHandler> _logger;

    public RefreshTokensCommandHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        [FromKeyedServices(UnitOfWorkContext.Accounts)]
        IUnitOfWork unitOfWork,
        ILogger<RefreshTokensCommandHandler> logger)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LoginResponse, ErrorList>> HandleAsync(
        RefreshTokensCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting token refresh process for token pair");

        // Получаем и валидируем refresh session
        var refreshSessionResult = await _refreshSessionManager
            .GetRefreshSessionByTokenAsync(command.RefreshToken, cancellationToken);

        if (refreshSessionResult.IsFailure)
        {
            _logger.LogError("Refresh session validation failed: {Error}", refreshSessionResult.Error);
            return refreshSessionResult.Error.ToErrorList();
        }

        if (refreshSessionResult.Value.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token expired at {Expiration}", refreshSessionResult.Value.ExpiresAt);
            return Errors.Authorization.ExpiredRefreshToken().ToErrorList();
        }

        // Валидируем access token
        var userClaimsResult = await _tokenProvider.GetUserClaimsFromTokenAsync(command.AccessToken);
        if (userClaimsResult.IsFailure)
        {
            _logger.LogError("Access token validation failed: {Error}", userClaimsResult.Error);
            return userClaimsResult.Error.ToErrorList();
        }

        // Проверяем соответствие claim-ов
        var userIdClaimValue = userClaimsResult
            .Value
            .FirstOrDefault(uc => uc.Type == CustomClaims.NAME_IDENTIFIER)?.Value;

        if (!Guid.TryParse(userIdClaimValue, out var userId))
        {
            _logger.LogError("Failed to get userId claim from token: {AccessToken}", command.AccessToken);
            return Errors.Authorization.InvalidAccessToken().ToErrorList();
        }

        if (refreshSessionResult.Value.UserId != userId)
        {
            _logger.LogError("User Id mismatch between tokens. Refresh: {RefreshUserId}, Access: {AccessUserId}", 
                refreshSessionResult.Value.UserId, userIdClaimValue);
            return Errors.Authorization.TokenMismatch().ToErrorList();
        }

        var jtiClaimValue = userClaimsResult
            .Value
            .FirstOrDefault(uc => uc.Type == CustomClaims.JTI)?.Value;

        if (!Guid.TryParse(jtiClaimValue, out var userJti))
        {
            _logger.LogError("Failed to get jti claim from token: {AccessToken}", command.AccessToken);
            return Errors.Authorization.InvalidAccessToken().ToErrorList();
        }

        if (refreshSessionResult.Value.Jti != userJti)
        {
            _logger.LogError("JTI mismatch between tokens. Refresh: {RefreshJti}, Access: {AccessJti}",
                refreshSessionResult.Value.Jti, jtiClaimValue);
            return Errors.Authorization.TokenMismatch().ToErrorList();
        }

        // Обновление токенов
        _refreshSessionManager.DeleteRefreshSession(refreshSessionResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var newAccessToken = _tokenProvider.GenerateAccessToken(refreshSessionResult.Value.User);
        var refreshToken = await _tokenProvider
            .GenerateRefreshTokenAsync(refreshSessionResult.Value.User, newAccessToken.Jti, cancellationToken);

        _logger.LogInformation("Tokens refreshed successfully for user {UserId}", userId);

        return new LoginResponse(newAccessToken.AccessToken, refreshToken);
    }
}