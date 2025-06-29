namespace PetFamily.Accounts.Domain.DataModels;

public class Permission
{
    public Guid Id { get; init; }
    public string Code { get; init; } = null!;

    public List<RolePermission> RolePermissions { get; init; } = [];
}