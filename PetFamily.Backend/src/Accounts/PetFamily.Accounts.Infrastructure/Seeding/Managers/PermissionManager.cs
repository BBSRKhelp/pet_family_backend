using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Seeding.Managers;

public class PermissionManager
{
    private readonly AccountsDbContext _accountsContext;
    private readonly ILogger<PermissionManager> _logger;

    public PermissionManager(
        AccountsDbContext accountsContext,
        ILogger<PermissionManager> logger)
    {
        _accountsContext = accountsContext;
        _logger = logger;
    }

    public async Task SyncPermissionsAsync(
        List<string> permissions,
        CancellationToken cancellationToken = default)
    {
        var existingPermissions = await _accountsContext
            .Permissions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var existingCodes = existingPermissions
            .Select(p => p.Code)
            .ToHashSet();

        var codesToAdd = permissions
            .Except(existingCodes)
            .ToList();

        var codesToRemove = existingPermissions
            .Where(p => !permissions.Contains(p.Code))
            .ToList();

        if (codesToAdd.Count > 0)
        {
            var newPermissions = codesToAdd
                .Select(code => new Permission { Code = code })
                .ToList();

            await _accountsContext.Permissions.AddRangeAsync(newPermissions, cancellationToken);
        }

        if (codesToRemove.Count > 0)
        {
            _accountsContext.Permissions.RemoveRange(codesToRemove);
        }

        await _accountsContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{CodeAdd} codes were added and {CodeRemove} codes were removed",
            codesToAdd.Count, codesToRemove.Count);
    }

    public async Task<Permission?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _accountsContext
            .Permissions
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissionCodesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _accountsContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => _accountsContext.RolePermissions
                .Where(rp => rp.RoleId == ur.RoleId)
                .Select(rp => rp.Permission.Code))
            .AsNoTracking()
            .ToHashSetAsync(cancellationToken);
    }
}