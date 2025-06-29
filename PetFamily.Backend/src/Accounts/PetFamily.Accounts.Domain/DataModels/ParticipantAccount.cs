namespace PetFamily.Accounts.Domain.DataModels;

public class ParticipantAccount
{
    public const string PARTICIPANT = nameof(PARTICIPANT);
    private readonly List<Guid> _favoritePetIds = [];

    //ef core
    private ParticipantAccount() { }

    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }

    public Guid Id { get; init; }
    public IReadOnlyList<Guid> FavoritePetIds => _favoritePetIds;
    
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    public void AddFavoritePet(Guid petId)
    {
        _favoritePetIds.Add(petId);
    }
}