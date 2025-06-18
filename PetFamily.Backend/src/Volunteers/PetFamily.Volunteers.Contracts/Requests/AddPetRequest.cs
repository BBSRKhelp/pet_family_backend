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
    DateOnly? BirthDate,
    string Status,
    Guid SpeciesId,
    Guid BreedId);