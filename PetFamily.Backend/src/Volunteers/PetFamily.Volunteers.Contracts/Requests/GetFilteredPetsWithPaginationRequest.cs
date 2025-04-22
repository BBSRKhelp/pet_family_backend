namespace PetFamily.Volunteers.Contracts.Requests;

public record GetFilteredPetsWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? Name,
    string? Coloration,
    float? Weight,
    float? Height,
    string? Country,
    string? City,
    string? Street,
    string? PostalCode,
    string? PhoneNumber,
    DateTime? BirthDate,
    string? Status,
    bool? IsCastrated,
    bool? IsVaccinated,
    int? Position,
    Guid? VolunteerId,
    Guid? SpeciesId,
    Guid? BreedId,
    string? SortBy,
    string? SortDirection);