using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Seeding.Managers;

public class RolePermissionManager
{
    private readonly AccountsDbContext _accountsContext;
    private readonly ILogger<RolePermissionManager> _logger;

    public RolePermissionManager(
        AccountsDbContext accountsContext,
        ILogger<RolePermissionManager> logger)
    {
        _accountsContext = accountsContext;
        _logger = logger;
    }

    public async Task SyncRolesAndPermissionsAsync(
        Dictionary<string, string[]> rolesWithPermissions,
        CancellationToken cancellationToken = default)
    {
        var roleNames = rolesWithPermissions.Keys.ToList();

        var roles = await _accountsContext.Roles
            .Where(r => roleNames.Contains(r.Name!))
            .ToListAsync(cancellationToken);

        var rolesByNameDictionary = roles.ToDictionary(r => r.Name!);

        var allPermissionCodes = rolesWithPermissions
            .SelectMany(rwp => rwp.Value)
            .ToList();

        var permissions = await _accountsContext.Permissions
            .Where(p => allPermissionCodes.Contains(p.Code))
            .ToListAsync(cancellationToken);

        var permissionsByCodeDictionary = permissions.ToDictionary(p => p.Code);

        var existingRolePermissions = await _accountsContext.RolePermissions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var requiredRolePermissions = new List<RolePermission>();

        foreach (var (roleName, permissionCodes) in rolesWithPermissions)
        {
            if (!rolesByNameDictionary.TryGetValue(roleName, out var role))
                continue;

            foreach (var permissionCode in permissionCodes)
            {
                if (!permissionsByCodeDictionary.TryGetValue(permissionCode, out var permission))
                    continue;

                requiredRolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                });
            }
        }

        var toAdd = requiredRolePermissions
            .Where(rp => !existingRolePermissions
                .Any(existing => existing.RoleId == rp.RoleId && existing.PermissionId == rp.PermissionId))
            .ToList();

        var toRemove = existingRolePermissions
            .Where(existing => !requiredRolePermissions
                .Any(rp => rp.RoleId == existing.RoleId && rp.PermissionId == existing.PermissionId))
            .ToList();

        if (toAdd.Count > 0)
            await _accountsContext.RolePermissions.AddRangeAsync(toAdd, cancellationToken);

        if (toRemove.Count > 0)
            _accountsContext.RolePermissions.RemoveRange(toRemove);

        await _accountsContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role-permission mapping synced: added {Added}, removed {Removed}.",
            toAdd.Count, toRemove.Count);
    }
}