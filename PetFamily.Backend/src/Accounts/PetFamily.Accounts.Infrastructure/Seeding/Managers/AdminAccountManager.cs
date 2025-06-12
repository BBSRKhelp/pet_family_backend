using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Seeding.Managers;

public class AdminAccountManager
{
    private readonly AccountsDbContext _accountsDbContext;
    private readonly ILogger<AdminAccountManager> _logger;

    public AdminAccountManager(
        AccountsDbContext accountsDbContext, 
        ILogger<AdminAccountManager> logger)
    {
        _accountsDbContext = accountsDbContext;
        _logger = logger;
    }
    
    public async Task CreateAdminAccount(
        AdminAccount adminAccount,
        CancellationToken cancellationToken = default)
    {
        await _accountsDbContext.AdminAccounts.AddAsync(adminAccount, cancellationToken);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Admin Accounts added to database");
    }
}