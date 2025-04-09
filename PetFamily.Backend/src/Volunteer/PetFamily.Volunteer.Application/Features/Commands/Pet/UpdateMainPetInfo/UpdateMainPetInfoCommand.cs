using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Pet;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.UpdateMainPetInfo;

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