using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract(PermissionManager permissionManager) : IAccountsContract
{
    public async Task<HashSet<string>> GetUserPermissionCodesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await permissionManager.GetUserPermissionCodesAsync(userId, cancellationToken);
    }
}