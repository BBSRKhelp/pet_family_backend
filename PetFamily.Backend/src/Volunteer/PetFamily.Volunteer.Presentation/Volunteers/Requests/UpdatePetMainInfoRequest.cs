using PetFamily.Core.Enums;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdateMainPetInfo;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Pet;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record UpdatePetMainInfoRequest(
    string? Name,
    string? Description,
    string Coloration,
    float Weight,
    float Height,
    string Country,
    string City,
    string Street,
    string? PostalCode,
    string PhoneNumber,
    DateTime? BirthDate,
    string HealthInformation,
    bool IsCastrated,
    bool IsVaccinated,
    IEnumerable<RequisiteDto>? Requisites,
    Guid SpeciesId,
    Guid BreedId)
{
    public UpdateMainPetInfoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        var coloration = Enum.TryParse(Coloration, true, out Colour resultColour)
            ? resultColour 
            : Colour.Unknown;

        var appearanceDetails = new AppearanceDetailsDto(coloration, Weight, Height);

        var address = new AddressDto(Country, City, Street, PostalCode);

        var healthDetails = new HealthDetailsDto(HealthInformation, IsCastrated, IsVaccinated);

        var breedAndSpeciesId = new BreedAndSpeciesIdDto(SpeciesId, BreedId);

        return new UpdateMainPetInfoCommand(
            volunteerId,
            petId,
            Name,
            Description,
            appearanceDetails,
            address,
            PhoneNumber,
            BirthDate,
            healthDetails,
            Requisites,
            breedAndSpeciesId);
    }
}