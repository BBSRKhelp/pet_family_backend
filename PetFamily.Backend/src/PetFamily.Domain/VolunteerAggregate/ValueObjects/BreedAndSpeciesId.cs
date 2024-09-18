using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record BreedAndSpeciesId
{
    private BreedAndSpeciesId(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    public SpeciesId SpeciesId { get; } = null!;
    public Guid BreedId { get; }

    public static Result<BreedAndSpeciesId> Create(
        SpeciesId speciesId, 
        Guid breedId)
    {
        if (speciesId == SpeciesId.Empty())
            return Result.Failure<BreedAndSpeciesId>("Species Id cannot be empty.");
        
        if (breedId == Guid.Empty)
            return Result.Failure<BreedAndSpeciesId>("Breed Id cannot be empty.");
        
        var breedAndSpeciesId = new BreedAndSpeciesId(speciesId, breedId);
        
        return Result.Success(breedAndSpeciesId);
    }
}