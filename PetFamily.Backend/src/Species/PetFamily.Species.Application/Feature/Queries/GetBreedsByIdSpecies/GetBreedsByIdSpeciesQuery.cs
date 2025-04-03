using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Queries.GetBreedsByIdSpecies;

public record GetBreedsByIdSpeciesQuery(
    Guid SpeciesId,
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;