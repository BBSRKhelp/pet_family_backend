namespace PetFamily.Accounts.Domain.DataModels;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);

    //ef core
    private AdminAccount() { }

    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }

    public Guid Id { get; init; }

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}