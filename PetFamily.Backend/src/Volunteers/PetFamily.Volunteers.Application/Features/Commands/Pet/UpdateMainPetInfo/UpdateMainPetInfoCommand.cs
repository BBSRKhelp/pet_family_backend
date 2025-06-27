using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.UpdateMainPetInfo;

public record UpdateMainPetInfoCommand(
    Guid VolunteerId,
    Guid PetId,
    string? Name,
    string? Description,
    AppearanceDetailsDto AppearanceDetails,
    AddressDto Address,
    DateOnly? BirthDate,
    HealthDetailsDto HealthDetails,
    BreedAndSpeciesIdDto BreedAndSpeciesId) : ICommand;