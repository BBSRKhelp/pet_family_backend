namespace PetFamily.Species.Contracts.DTOs;

public record SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
}