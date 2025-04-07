using PetFamily.Species.Application.Feature.Queries.GetBreedsByIdSpecies;

namespace PetFamily.Species.Presentation.Breeds.Requests;

public record GetBreedsByIdSpeciesRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? SortBy,
    string? SortDirection)
{
    public GetBreedsByIdSpeciesQuery ToQuery(Guid speciesId)
    {
        return new GetBreedsByIdSpeciesQuery(
            speciesId,
            PageNumber,
            PageSize,
            Name,
            SortBy ?? "id",
            SortDirection ?? "ASC");
    }
}