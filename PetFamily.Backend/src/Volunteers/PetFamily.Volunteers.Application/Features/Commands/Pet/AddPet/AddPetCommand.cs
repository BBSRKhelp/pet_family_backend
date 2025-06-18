using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string? Name,
    string? Description,
    AppearanceDetailsDto AppearanceDetails,
    HealthDetailsDto HealthDetails,
    AddressDto Address,
    DateOnly? BirthDate,
    Status Status,
    BreedAndSpeciesIdDto BreedAndSpeciesId) : ICommand;