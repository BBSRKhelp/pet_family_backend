using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerAggregate.Entities;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record PetDetails
{
    private readonly List<PetPhoto> _petPhotos = [];
    private readonly List<Requisite> _requisites = [];

    //ef core
    private PetDetails()
    {
    }

    public PetDetails(IEnumerable<PetPhoto> petPhoto, IEnumerable<Requisite> requisite)
    {
        _petPhotos.AddRange(petPhoto);
        _requisites.AddRange(requisite);
    }

    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;
    public IReadOnlyList<Requisite> Requisites => _requisites;
}