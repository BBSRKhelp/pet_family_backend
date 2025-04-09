using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteer.Domain.ValueObjects;

public record PetPhoto
{
    //ef core
    private PetPhoto()
    {
    }

    public PetPhoto(PhotoPath photoPath, bool isMainPhoto = false)
    {
        PhotoPath = photoPath;
        IsMainPhoto = isMainPhoto;
    }

    public PhotoPath PhotoPath { get; } = null!;
    public bool IsMainPhoto { get; }
}