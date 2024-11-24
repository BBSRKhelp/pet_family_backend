namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

public record SocialNetworksShell
{
    //ef core
    private SocialNetworksShell()
    {
    }

    public SocialNetworksShell(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }

    public IReadOnlyList<SocialNetwork> SocialNetworks { get; } = null!;
}