using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetBreedsByIdSpecies;

public record GetBreedsByIdSpeciesQuery(
    Guid SpeciesId,
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;