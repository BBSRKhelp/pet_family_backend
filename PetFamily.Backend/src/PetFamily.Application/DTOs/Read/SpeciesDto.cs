namespace PetFamily.Application.DTOs.Read;

public record SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
}