using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly AccountsDbContext _accountsContext;
    private readonly ILogger<RefreshSessionManager> _logger;

    public RefreshSessionManager(
        AccountsDbContext accountsContext,
        ILogger<RefreshSessionManager> logger)
    {
        _accountsContext = accountsContext;
        _logger = logger;
    }

    public async Task<Result<RefreshSession, Error>> GetRefreshSessionByTokenAsync(
        Guid refreshToken,
        CancellationToken cancellationToken = default)
    {
        var refreshSession = await _accountsContext
            .RefreshSessions
            .Include(rs => rs.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(rs => rs.Token == refreshToken, cancellationToken);

        if (refreshSession is null)
        {
            _logger.LogWarning("Refresh token not found: {Token}", refreshToken);
            return Errors.Authorization.InvalidRefreshToken();
        }

        _logger.LogInformation("Refresh token {RefreshToken} found", refreshToken);
        return refreshSession;
    }

    public async Task<RefreshSession?> GetRefreshSessionByUserAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var refreshSession = await _accountsContext
            .RefreshSessions
            .FirstOrDefaultAsync(rs => rs.User == user, cancellationToken);

        if (refreshSession is null)
        {
            _logger.LogInformation("Refresh token for user {UserId} not found", user.Id);
            return null;
        }

        _logger.LogInformation("Refresh token {UserId} found", user.Id);
        return refreshSession;
    }

    public void DeleteRefreshSession(RefreshSession refreshSession) 
        => _accountsContext.RefreshSessions.Remove(refreshSession);
}