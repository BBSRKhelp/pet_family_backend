using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers;

public class VolunteerAccountManager
{
    private readonly AccountsDbContext _accountsDbContext;
    private readonly ILogger<VolunteerAccountManager> _logger;

    public VolunteerAccountManager(
        AccountsDbContext accountsDbContext, 
        ILogger<VolunteerAccountManager> logger)
    {
        _accountsDbContext = accountsDbContext;
        _logger = logger;
    }
    
    public async Task CreateVolunteerAccountAsync(
        VolunteerAccount volunteerAccount,
        CancellationToken cancellationToken = default)
    {
        await _accountsDbContext.VolunteerAccounts.AddAsync(volunteerAccount, cancellationToken);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Volunteer account added to database");
    }
}