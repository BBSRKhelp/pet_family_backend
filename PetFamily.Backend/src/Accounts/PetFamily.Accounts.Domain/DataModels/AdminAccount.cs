using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain.DataModels;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);

    //ef core
    private AdminAccount() { }

    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        User = user;
    }

    public Guid Id { get; init; }
    public FullName FullName { get; init; } = null!;

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}