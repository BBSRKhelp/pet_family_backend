using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers;

public class AdminAccountManager(AccountsDbContext accountsDbContext)
{
    public async Task CreateAdminAccountAsync(
        AdminAccount adminAccount,
        CancellationToken cancellationToken = default)
    {
        await accountsDbContext.AdminAccounts.AddAsync(adminAccount, cancellationToken);
        await accountsDbContext.SaveChangesAsync(cancellationToken);
    }
}