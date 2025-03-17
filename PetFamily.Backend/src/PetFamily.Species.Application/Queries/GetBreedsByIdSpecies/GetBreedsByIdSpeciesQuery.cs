using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedsByIdSpecies;

public record GetBreedsByIdSpeciesQuery(
    Guid SpeciesId,
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;