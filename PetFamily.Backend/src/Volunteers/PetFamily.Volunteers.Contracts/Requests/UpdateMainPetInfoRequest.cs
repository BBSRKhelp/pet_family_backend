using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests;

public record UpdateMainPetInfoRequest(
    string? Name,
    string? Description,
    string Coloration,
    float Weight,
    float Height,
    string Country,
    string City,
    string Street,
    string? PostalCode,
    string PhoneNumber,
    DateOnly? BirthDate,
    string HealthInformation,
    bool IsCastrated,
    bool IsVaccinated,
    IEnumerable<RequisiteDto>? Requisites,
    Guid SpeciesId,
    Guid BreedId);