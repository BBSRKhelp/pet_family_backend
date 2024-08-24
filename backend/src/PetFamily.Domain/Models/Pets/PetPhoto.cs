namespace PetFamily.Domain.Models.Pets;

public record PetPhoto
{
    public string? Path { get; }

    public bool? MainPhoto { get; }
}