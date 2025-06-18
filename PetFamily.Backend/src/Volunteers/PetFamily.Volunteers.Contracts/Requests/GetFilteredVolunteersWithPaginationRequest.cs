
namespace PetFamily.Volunteers.Contracts.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? SortBy,
    string? SortDirection);