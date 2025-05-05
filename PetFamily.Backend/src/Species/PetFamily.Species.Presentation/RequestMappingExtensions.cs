using PetFamily.Species.Application.Feature.Commands.Breed.AddBreed;
using PetFamily.Species.Application.Feature.Commands.Species.Create;
using PetFamily.Species.Application.Feature.Queries.GetBreedsByIdSpecies;
using PetFamily.Species.Application.Feature.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation;

public static class RequestMappingExtensions
{
    public static CreateSpeciesCommand ToCommand(this CreateSpeciesRequest request)
    {
        return new CreateSpeciesCommand(request.Name);
    }

    public static AddBreedCommand ToCommand(this AddBreedRequest request, Guid id)
    {
        return new AddBreedCommand(id, request.Name);
    }

    public static GetFilteredSpeciesWithPaginationQuery ToQuery(this GetFilteredSpeciesWithPaginationRequest request)
    {
        return new GetFilteredSpeciesWithPaginationQuery(
            request.PageNumber,
            request.PageSize,
            request.Name,
            request.SortBy ?? "id",
            request.SortDirection ?? "ASC");
    }

    public static GetBreedsByIdSpeciesQuery ToQuery(this GetBreedsByIdSpeciesRequest request, Guid speciesId)
    {
        return new GetBreedsByIdSpeciesQuery(
            speciesId,
            request.PageNumber,
            request.PageSize,
            request.Name,
            request.SortBy ?? "id",
            request.SortDirection ?? "ASC");
    }
}