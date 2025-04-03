namespace PetFamily.Species.Contracts.Requests;

public record GetBreedsByIdSpeciesRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? SortBy,
    string? SortDirection)
{
    /*public GetBreedsByIdSpeciesQuery ToQuery(Guid speciesId)
    {
        return new GetBreedsByIdSpeciesQuery(
            speciesId,
            PageNumber,
            PageSize,
            Name,
            SortBy ?? "id",
            SortDirection ?? "ASC");
    }*/
}