namespace PetFamily.Accounts.Contracts;

public interface IAccountsContract
{
    Task<HashSet<string>> GetUserPermissionCodesAsync(Guid userId, CancellationToken cancellationToken = default);
}