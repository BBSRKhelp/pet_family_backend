using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Infrastructure.Seeding.Managers;

namespace PetFamily.Accounts.Presentation;

public class AccountsContract : IAccountsContract
{
    private readonly PermissionManager _permissionManager;

    public AccountsContract(PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }
    
    public async Task<HashSet<string>> GetUserPermissionCodesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _permissionManager.GetUserPermissionCodesAsync(userId, cancellationToken);
    }
}