using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Queries.GetFilteredSpeciesWithPagination;

public record GetFilteredSpeciesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;