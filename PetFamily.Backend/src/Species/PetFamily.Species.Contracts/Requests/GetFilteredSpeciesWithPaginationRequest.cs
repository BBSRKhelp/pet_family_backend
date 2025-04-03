
namespace PetFamily.Species.Contracts.Requests;

public record GetFilteredSpeciesWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? SortBy,
    string? SortDirection)
{
    /*public GetFilteredSpeciesWithPaginationQuery ToQuery()
    {
        return new GetFilteredSpeciesWithPaginationQuery(
            PageNumber,
            PageSize,
            Name,
            SortBy ?? "id",
            SortDirection ?? "ASC");
    }*/
}