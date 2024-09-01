namespace PetFamily.Domain.Models.Pets;

public record PetPhoto
{
    private PetPhoto(string path, bool isMainPhoto)
    {
        Path = path;
        IsMainPhoto = isMainPhoto;
    }
    
    public string? Path { get; }
    public bool? IsMainPhoto { get; }
}