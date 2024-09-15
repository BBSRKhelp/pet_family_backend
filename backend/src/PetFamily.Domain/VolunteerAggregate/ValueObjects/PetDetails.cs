using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerAggregate.Entities;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record PetDetails
{
    //ef core
    private PetDetails()
    {
    }
    
    private PetDetails(List<PetPhoto> petPhoto, List<Requisite> requisite)
    {
        PetPhotos = petPhoto;
        Requisites = requisite;
    }
    
    public List<PetPhoto>? PetPhotos { get; }
    public List<Requisite> Requisites { get; }

    public static Result<PetDetails> Create(List<PetPhoto> petPhoto, List<Requisite> requisite)
    {
        //TODO Валидация нужна ли?
        
        var petDetails = new PetDetails(petPhoto, requisite);
        
        return Result.Success(petDetails);
    }
}