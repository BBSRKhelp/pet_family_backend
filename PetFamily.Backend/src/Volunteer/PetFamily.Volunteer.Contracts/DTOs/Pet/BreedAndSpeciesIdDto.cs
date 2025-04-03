using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Volunteer.Contracts.DTOs.Pet;

public record BreedAndSpeciesIdDto(SpeciesId SpeciesId, Guid BreedId);