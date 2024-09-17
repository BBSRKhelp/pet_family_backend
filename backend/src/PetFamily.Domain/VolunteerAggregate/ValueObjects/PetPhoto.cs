using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record PetPhoto
{
    //ef core
    private PetPhoto()
    {
    }
    private PetPhoto(string path, bool isMainPhoto)
    {
        Path = path;
        IsMainPhoto = isMainPhoto;
    }
    
    public string Path { get; }
    public bool? IsMainPhoto { get; }

    public static Result<PetPhoto> Create(string path, bool isMainPhoto)
    {
        if(IsNullOrEmpty(path))
            return Result.Failure<PetPhoto>("Path cannot be null or empty");
        
        var petPhoto = new PetPhoto(path, isMainPhoto);
        
        return Result.Success(petPhoto);
    }
}