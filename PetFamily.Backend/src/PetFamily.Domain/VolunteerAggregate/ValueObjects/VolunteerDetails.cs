using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record VolunteerDetails
{
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<Requisite> _requisites = [];
    
    //ef core
    private VolunteerDetails()
    {
    }
    
    public VolunteerDetails(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
    {
        _socialNetworks.AddRange(socialNetworks);
        _requisites.AddRange(requisites);
    }
    
    public IEnumerable<SocialNetwork> SocialNetworks => _socialNetworks;
    public IEnumerable<Requisite> Requisites => _requisites;
}