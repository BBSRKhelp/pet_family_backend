namespace PetFamily.Species.Application.DTOs.Read;

public class BreedDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public Guid SpeciesId { get; init; }
}