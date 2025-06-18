using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string SortBy,
    string SortDirection) : IQuery;
