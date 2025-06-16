using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.UpdateMainPetInfo;

public record UpdateMainPetInfoCommand(
    Guid VolunteerId,
    Guid PetId,
    string? Name,
    string? Description,
    AppearanceDetailsDto AppearanceDetails,
    AddressDto Address,
    string PhoneNumber,
    DateOnly? BirthDate,
    HealthDetailsDto HealthDetails,
    IEnumerable<RequisiteDto>? Requisites,
    BreedAndSpeciesIdDto BreedAndSpeciesId) : ICommand;