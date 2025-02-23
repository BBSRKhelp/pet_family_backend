using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.VolunteerAggregate.Queries.Pet.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
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
    string SortBy,
    string SortDirection) : IQuery;