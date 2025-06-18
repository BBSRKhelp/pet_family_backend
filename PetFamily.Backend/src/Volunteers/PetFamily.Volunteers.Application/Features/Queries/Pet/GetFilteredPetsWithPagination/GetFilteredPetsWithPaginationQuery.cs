using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;

namespace PetFamily.Volunteers.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? Name,
    Colour? Coloration,
    float? Weight,
    float? Height,
    string? Country,
    string? City,
    string? Street,
    string? PostalCode,
    DateTime? BirthDate,
    Status? Status,
    bool? IsCastrated,
    bool? IsVaccinated,
    int? Position,
    Guid? VolunteerId,
    Guid? BreedId,
    Guid? SpeciesId,
    string SortBy,
    string SortDirection) : IQuery;