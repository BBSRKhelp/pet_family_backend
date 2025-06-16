using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests;

public record AddPetRequest(
    string? Name,
    string? Description,
    string Coloration,
    float Weight,
    float Height,
    string HealthInformation,
    bool IsCastrated,
    bool IsVaccinated,
    string Country,
    string City,
    string Street,
    string? PostalCode,
    string PhoneNumber,
    DateOnly? BirthDate,
    string Status,
    IEnumerable<RequisiteDto>? Requisites,
    Guid SpeciesId,
    Guid BreedId);