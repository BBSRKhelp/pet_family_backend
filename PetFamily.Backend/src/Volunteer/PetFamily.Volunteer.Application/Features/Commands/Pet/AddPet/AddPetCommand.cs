using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Pet;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string? Name,
    string? Description,
    AppearanceDetailsDto AppearanceDetails,
    HealthDetailsDto HealthDetails,
    AddressDto Address,
    string PhoneNumber,
    DateTime? BirthDate,
    Status Status,
    IEnumerable<RequisiteDto>? Requisites,
    BreedAndSpeciesIdDto BreedAndSpeciesId) : ICommand;