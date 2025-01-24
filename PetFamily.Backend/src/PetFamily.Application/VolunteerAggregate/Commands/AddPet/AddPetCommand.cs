using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pet;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.VolunteerAggregate.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    string Description,
    AppearanceDetailsDto AppearanceDetails,
    HealthDetailsDto HealthDetails,
    AddressDto Address,
    string PhoneNumber,
    DateOnly? Birthday,
    StatusForHelp Status,
    IEnumerable<RequisiteDto>? Requisites,
    BreedAndSpeciesIdDto BreedAndSpeciesId) : ICommand;