using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record PhotoPath
{
    private PhotoPath()
    {
    }

    private PhotoPath(string path)
    {
        Path = path;
    }

    public string Path { get; } = null!;

    public static Result<PhotoPath, Error> Create(string photoPath)
    {
        if (string.IsNullOrWhiteSpace(photoPath))
            return Errors.General.IsRequired(nameof(photoPath));

        //var extension = System.IO.PhotoPath.GetExtension(photoPath);
        // if (extension is not (".jpg" or ".jpeg" or ".png"))
        //     return Errors.General.IsInvalid(nameof(extension));

        return new PhotoPath(photoPath);
    }
}