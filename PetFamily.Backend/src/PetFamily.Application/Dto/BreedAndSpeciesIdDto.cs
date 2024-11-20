using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Application.Dto;

public record BreedAndSpeciesIdDto(SpeciesId SpeciesId, Guid BreedId);