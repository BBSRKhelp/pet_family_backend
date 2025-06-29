namespace PetFamily.Accounts.Infrastructure.Options;

public class JwtOptions
{
    public const string JWT = nameof(JWT);
    
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Key { get; init; } = null!;
    public int AccessTokenLifeTimeM { get; init; }
}