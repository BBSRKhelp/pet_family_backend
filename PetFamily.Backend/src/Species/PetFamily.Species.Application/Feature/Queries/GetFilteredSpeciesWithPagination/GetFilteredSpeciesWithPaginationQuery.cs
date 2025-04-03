using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Queries.GetFilteredSpeciesWithPagination;

public record GetFilteredSpeciesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;