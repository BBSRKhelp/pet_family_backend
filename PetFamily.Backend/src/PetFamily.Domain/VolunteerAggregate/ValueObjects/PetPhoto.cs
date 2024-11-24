using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record PetPhoto
{
    //ef core
    private PetPhoto()
    {
    }

    public PetPhoto(PhotoPath path, bool isMainPhoto = false)
    {
        Path = path;
        IsMainPhoto = isMainPhoto;
    }

    public PhotoPath Path { get; } = null!;
    public bool? IsMainPhoto { get; }
}