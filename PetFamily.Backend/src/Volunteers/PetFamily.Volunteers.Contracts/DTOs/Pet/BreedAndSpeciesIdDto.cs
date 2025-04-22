using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteers.Contracts.DTOs.Pet;

public record BreedAndSpeciesIdDto(SpeciesId SpeciesId, Guid BreedId);