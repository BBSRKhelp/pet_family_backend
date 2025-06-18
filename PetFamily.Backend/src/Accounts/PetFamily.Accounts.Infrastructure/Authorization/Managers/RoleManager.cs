using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers;

public class RoleManager : RoleManager<Role>
{
    private readonly AccountsDbContext _accountsContext;
    private readonly ILogger<RoleManager<Role>> _logger;

    public RoleManager(
        IRoleStore<Role> store, 
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        ILogger<RoleManager<Role>> logger, 
        AccountsDbContext accountsContext) 
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {
        _accountsContext = accountsContext;
        _logger = logger;
    }

    public async Task SyncRolesAsync(
        List<string> roles,
        CancellationToken cancellationToken = default)
    {
        var existingRoles = await _accountsContext
            .Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var existingNames = existingRoles
            .Select(p => p.Name)
            .ToHashSet();
        
        var rolesToAdd = roles
            .Except(existingNames)
            .ToList();

        var rolesToRemove = existingRoles
            .Where(r => !roles.Contains(r.Name!))
            .ToList();

        if (rolesToAdd.Count > 0)
        {
            var newRoles = rolesToAdd
                .Select(name => new Role { Name = name, NormalizedName = name!.ToUpper() })
                .ToList();
            
            await _accountsContext.Roles.AddRangeAsync(newRoles, cancellationToken);
        }

        if (rolesToRemove.Count > 0)
        {
            _accountsContext.Roles.RemoveRange(rolesToRemove);
        }

        await _accountsContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{RolesAdd} roles were added and {RolesRemove} roles were removed",
            rolesToAdd.Count, rolesToRemove.Count);
    }
}