namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

public record SocialNetworkShell
{
    //ef core
    private SocialNetworkShell()
    {
    }

    public SocialNetworkShell(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }

    public IReadOnlyList<SocialNetwork> SocialNetworks { get; } = null!;
}