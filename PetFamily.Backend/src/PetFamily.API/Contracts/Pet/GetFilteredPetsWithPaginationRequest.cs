using PetFamily.Application.VolunteerAggregate.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.API.Contracts.Pet;

public record GetFilteredPetsWithPaginationRequest(
    int PageNumber,
    int PageSize,
    Guid? VolunteerId,
    string? Name,
    byte? Age,
    Guid? BreedId,
    Colour? Colouration,
    Guid? SpeciesId,
    string? Country,
    string? City,
    string? Street,
    string? PostalCode,
    //...
    string? SortBy,
    string? SortDirection)
{
    public GetFilteredPetsWithPaginationQuery ToQuery()
    {
        return new GetFilteredPetsWithPaginationQuery(
            PageNumber, 
            PageSize,
            VolunteerId,
            Name,
            Age,
            BreedId,
            Colouration,
            SpeciesId,
            Country,
            City,
            Street,
            PostalCode,
            SortBy ?? "id",
            SortDirection ?? "ASC");
    }
}