using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Features.Commands.Login;

public class LoginHandler : ICommandHandler<LoginResponse, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        IRefreshSessionManager refreshSessionManager,
        [FromKeyedServices(UnitOfWorkContext.Accounts)]IUnitOfWork unitOfWork,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _refreshSessionManager = refreshSessionManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LoginResponse, ErrorList>> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Login attempt for email: {Email}", command.Email);

        // Поиск пользователя
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            _logger.LogWarning("Login failed - user not found: {Email}", command.Email);
            return Errors.General.NotFound("user").ToErrorList();
        }

        // Проверка учетных данных
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed - invalid password for: {Email}", command.Email);
            return Errors.User.InvalidCredentials().ToErrorList();
        }

        // Проверка блокировки аккаунта
        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Login blocked - account locked for: {Email}", command.Email);
            return (ErrorList)Errors.User.AccountLocked();
        }

        // Очистка предыдущей сессии
        var refreshSession = await _refreshSessionManager.GetRefreshSessionByUserAsync(user, cancellationToken);
        if (refreshSession is not null)
        {
            _refreshSessionManager.DeleteRefreshSession(refreshSession);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Генерация новых токенов
        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshTokenAsync(user, accessToken.Jti, cancellationToken);

        _logger.LogInformation("User {UserId} with email: {Email} logged in", user.Id, command.Email);

        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}