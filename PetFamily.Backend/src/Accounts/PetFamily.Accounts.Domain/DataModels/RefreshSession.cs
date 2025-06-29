namespace PetFamily.Accounts.Domain.DataModels;

public class RefreshSession
{
    //ef core
    private RefreshSession()
    {
    }
    
    public RefreshSession(Guid token,
        Guid jti,
        DateTime createdAt,
        DateTime expiresAt,
        User user)
    {
        Token = token;
        Jti = jti;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        User = user;
    }

    public Guid Id { get; init; }
    public Guid Token { get; init; }
    public Guid Jti { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}