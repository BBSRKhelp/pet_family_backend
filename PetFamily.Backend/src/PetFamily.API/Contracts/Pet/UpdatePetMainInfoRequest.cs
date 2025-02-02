using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pet;
using PetFamily.Application.VolunteerAggregate.Commands.Pet.UpdateMainPetInfo;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.API.Contracts.Pet;

public record UpdatePetMainInfoRequest(
    string? Name,
    string? Description,
    string Colouration,
    float Weight,
    float Height,
    string Country,
    string City,
    string Street,
    string? PostalCode,
    string PhoneNumber,
    DateOnly? Birthday,
    string HealthInformation,
    bool IsCastrated,
    bool IsVaccinated,
    IEnumerable<RequisiteDto>? Requisites,
    Guid SpeciesId,
    Guid BreedId)
{
    public UpdatePetMainInfoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        var coloration = Enum.Parse<Colour>(Colouration, true);
        
        var appearanceDetails = new AppearanceDetailsDto(coloration, Weight, Height);

        var address = new AddressDto(Country, City, Street, PostalCode);

        var healthDetails = new HealthDetailsDto(HealthInformation, IsCastrated, IsVaccinated);

        var breedAndSpeciesId = new BreedAndSpeciesIdDto(SpeciesId, BreedId);

        return new UpdatePetMainInfoCommand(
            volunteerId,
            petId,
            Name,
            Description,
            appearanceDetails,
            address,
            PhoneNumber,
            Birthday,
            healthDetails,
            Requisites,
            breedAndSpeciesId);
    }
}