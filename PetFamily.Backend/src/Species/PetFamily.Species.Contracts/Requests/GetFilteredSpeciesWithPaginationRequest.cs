
namespace PetFamily.Species.Contracts.Requests;

public record GetFilteredSpeciesWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? SortBy,
    string? SortDirection);