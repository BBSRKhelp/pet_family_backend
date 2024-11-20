using PetFamily.Application.Dto;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.Commands.Volunteer.AddPet;

public record PetCreateCommand(
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
    BreedAndSpeciesIdDto BreedAndSpeciesId);