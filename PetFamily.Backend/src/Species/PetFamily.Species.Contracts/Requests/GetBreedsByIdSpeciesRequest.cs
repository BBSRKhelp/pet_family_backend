namespace PetFamily.Species.Contracts.Requests;

public record GetBreedsByIdSpeciesRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? SortBy,
    string? SortDirection);