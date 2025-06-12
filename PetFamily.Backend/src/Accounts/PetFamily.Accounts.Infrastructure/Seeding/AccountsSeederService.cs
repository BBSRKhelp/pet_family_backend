using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Seeding.Managers;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    UserManager<User> userManager,
    RoleManager roleManager,
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    AdminAccountManager adminAccountManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeederService> logger)
{
    private readonly AdminOptions _adminOptions = adminOptions.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding accounts...");

        var json = await File.ReadAllTextAsync(FilePaths.ACCOUNTS, cancellationToken);

        var seedData = JsonSerializer.Deserialize<RolePermissionJsonModel>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission configuration.");

        await SeedPermissionsAsync(seedData, cancellationToken);
        await SeedRolesAsync(seedData, cancellationToken);
        await SeedRolePermissionsAsync(seedData, cancellationToken);
        await SeedAdminAccountAsync(cancellationToken);

        logger.LogInformation("Everything seeded");
    }

    private async Task SeedPermissionsAsync(
        RolePermissionJsonModel seedData,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding Permissions");

        var permissions = seedData
            .Permissions
            .SelectMany(permissionGroup => permissionGroup.Value)
            .ToList();

        await permissionManager.SyncPermissionsAsync(permissions, cancellationToken);
    }

    private async Task SeedRolesAsync(
        RolePermissionJsonModel seedData,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding Roles");

        var roles = seedData.Roles
            .Select(r => r.Key)
            .ToList();

        await roleManager.SyncRolesAsync(roles, cancellationToken);
    }

    private async Task SeedRolePermissionsAsync(
        RolePermissionJsonModel seedData,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding Role Permissions");

        var rolesWithPermissions = seedData.Roles;

        await rolePermissionManager.SyncRolesAndPermissionsAsync(rolesWithPermissions, cancellationToken);
    }

    private async Task SeedAdminAccountAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding Admin Account");

        var existsUser = await userManager.FindByEmailAsync(_adminOptions.Email);
        if (existsUser is not null)
        {
            logger.LogInformation("Admin account already exists");
            return;
        }
        
        var adminRole = await roleManager.FindByNameAsync(AdminAccount.ADMIN)
                        ?? throw new ApplicationException("Could not find admin role");

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole);
        await userManager.CreateAsync(adminUser, _adminOptions.Password);

        var fullName = FullName.Create(adminUser.UserName!, adminUser.UserName!).Value;
        var adminAccount = new AdminAccount(fullName, adminUser);
        
        await adminAccountManager.CreateAdminAccount(adminAccount, cancellationToken);
    }
}