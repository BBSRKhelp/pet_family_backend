namespace PetFamily.Accounts.Infrastructure.Authorization.Seeding;

public class RolePermissionJsonModel
{
    public Dictionary<string, string[]> Permissions { get; init; } = [];
    public Dictionary<string, string[]> Roles { get; init; } = [];
}