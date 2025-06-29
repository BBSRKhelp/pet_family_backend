using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Interfaces.Managers;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetRefreshSessionByTokenAsync(Guid refreshToken,
        CancellationToken cancellationToken = default);

    Task<RefreshSession?> GetRefreshSessionByUserAsync(User user,
        CancellationToken cancellationToken = default);
    
    void DeleteRefreshSession(RefreshSession refreshSession);
}