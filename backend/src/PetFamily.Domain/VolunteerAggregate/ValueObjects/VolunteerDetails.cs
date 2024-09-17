using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record VolunteerDetails
{
    //ef core
    private VolunteerDetails()
    {
    }
    
    public VolunteerDetails(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
    {
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
    
    public IEnumerable<SocialNetwork> SocialNetworks { get; }
    public IEnumerable<Requisite> Requisites { get; }
}