using PetFamily.Core.Enums;
using PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Pet;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record CreatePetRequest(
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
    DateTime? BirthDate,
    string Status,
    IEnumerable<RequisiteDto>? Requisites,
    Guid SpeciesId,
    Guid BreedId)
{
    public AddPetCommand ToCommand(Guid volunteerId)
    {
        var coloration = Enum.TryParse(Coloration, true, out Colour resultColour) 
            ? resultColour 
            : Colour.Unknown;

        var appearanceDetails = new AppearanceDetailsDto(coloration, Weight, Height);

        var healthDetails = new HealthDetailsDto(HealthInformation, IsCastrated, IsVaccinated);

        var address = new AddressDto(Country, City, Street, PostalCode);

        var status = Enum.TryParse(Status, true, out Status resultStatus)
            ? resultStatus
            : Core.Enums.Status.Unknown;

        var breedAndSpeciesId = new BreedAndSpeciesIdDto(SpeciesId, BreedId);

        return new AddPetCommand(
            volunteerId,
            Name,
            Description,
            appearanceDetails,
            healthDetails,
            address,
            PhoneNumber,
            BirthDate,
            status,
            Requisites,
            breedAndSpeciesId);
    }
}