using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record VolunteerDetails
{
    //ef core
    private VolunteerDetails()
    {
    }
    
    private VolunteerDetails(List<SocialNetwork> socialNetworks, List<Requisite> requisites)
    {
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
    
    public List<SocialNetwork> SocialNetworks { get; init; }
    public List<Requisite> Requisites { get; init; }

    public static Result<VolunteerDetails> Create(
        List<SocialNetwork> socialNetworks,
        List<Requisite> requisites)
    {
        //TODO Валидация нужна?
        var volunteerDetails = new VolunteerDetails(socialNetworks, requisites);
        return Result.Success(volunteerDetails);
    }
}