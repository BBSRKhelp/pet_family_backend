using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWithPagination;

public record GetFilteredSpeciesWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Name,
    string SortBy,
    string SortDirection) : IQuery;