using PetFamily.Core.ValueObjects.Ids;

namespace PetFamily.Application.DTOs.Pet;

public record BreedAndSpeciesIdDto(SpeciesId SpeciesId, Guid BreedId);