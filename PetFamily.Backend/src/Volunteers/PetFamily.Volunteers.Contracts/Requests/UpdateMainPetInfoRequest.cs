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
    DateOnly? BirthDate,
    string HealthInformation,
    bool IsCastrated,
    bool IsVaccinated,
    Guid SpeciesId,
    Guid BreedId);