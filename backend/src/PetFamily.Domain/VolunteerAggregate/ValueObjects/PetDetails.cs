using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerAggregate.Entities;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record PetDetails
{
    //ef core
    private PetDetails()
    {
    }
    
    public PetDetails(IEnumerable<PetPhoto> petPhoto, IEnumerable<Requisite> requisite)
    {
        PetPhotos = petPhoto;
        Requisites = requisite;
    }
    
    public IEnumerable<PetPhoto>? PetPhotos { get; }
    public IEnumerable<Requisite> Requisites { get; }

}