namespace PetFamily.Accounts.Infrastructure.Options;

public class AdminOptions
{
    public const string ADMIN = "Admin";

    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}