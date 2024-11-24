using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Application.Dtos;

public record BreedAndSpeciesIdDto(SpeciesId SpeciesId, Guid BreedId);