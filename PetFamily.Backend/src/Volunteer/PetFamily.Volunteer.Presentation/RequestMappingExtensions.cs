using PetFamily.Core.Enums;
using PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;
using PetFamily.Volunteer.Application.Features.Commands.Pet.ChangePetsPosition;
using PetFamily.Volunteer.Application.Features.Commands.Pet.SetMainPetPhoto;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdateMainPetInfo;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdatePetStatus;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.Create;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateRequisites;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Volunteer.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Volunteer.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;
using PetFamily.Volunteer.Contracts.DTOs.Pet;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;
using PetFamily.Volunteer.Contracts.Requests;

namespace PetFamily.Volunteer.Presentation;

public static class RequestMappingExtensions
{
    public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
    {
        var fullName = new FullNameDto(request.FirstName, request.LastName, request.Patronymic);

        return new CreateVolunteerCommand(
            fullName,
            request.Email,
            request.Description,
            request.WorkExperience,
            request.PhoneNumber,
            request.SocialNetworks,
            request.Requisites);
    }

    public static UpdateMainVolunteerInfoCommand ToCommand(this UpdateMainInfoVolunteerRequest request, Guid id)
    {
        var fullName = new FullNameDto(request.FirstName, request.LastName, request.Patronymic);

        return new UpdateMainVolunteerInfoCommand(
            id,
            fullName,
            request.Email,
            request.Description,
            request.WorkExperience,
            request.PhoneNumber);
    }

    public static UpdateRequisitesVolunteerCommand ToCommand(this UpdateRequisitesVolunteerRequest request, Guid id)
    {
        return new UpdateRequisitesVolunteerCommand(id, request.Requisite);
    }

    public static UpdateSocialNetworksVolunteerCommand ToCommand(
        this UpdateSocialNetworksVolunteerRequest request,
        Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, request.SocialNetworks);
    }

    public static AddPetCommand ToCommand(this AddPetRequest request, Guid volunteerId)
    {
        var coloration = Enum.TryParse(request.Coloration, true, out Colour resultColour)
            ? resultColour
            : Colour.Unknown;

        var appearanceDetails = new AppearanceDetailsDto(coloration, request.Weight, request.Height);

        var healthDetails = new HealthDetailsDto(request.HealthInformation, request.IsCastrated, request.IsVaccinated);

        var address = new AddressDto(request.Country, request.City, request.Street, request.PostalCode);

        var status = Enum.TryParse(request.Status, true, out Status resultStatus)
            ? resultStatus
            : Status.Unknown;

        var breedAndSpeciesId = new BreedAndSpeciesIdDto(request.SpeciesId, request.BreedId);

        return new AddPetCommand(
            volunteerId,
            request.Name,
            request.Description,
            appearanceDetails,
            healthDetails,
            address,
            request.PhoneNumber,
            request.BirthDate,
            status,
            request.Requisites,
            breedAndSpeciesId);
    }


    public static ChangePetsPositionCommand ToCommand(
        this ChangePetsPositionRequest request,
        Guid volunteerId,
        Guid petId)
    {
        return new ChangePetsPositionCommand(volunteerId, petId, request.NewPosition);
    }

    public static UpdateMainPetInfoCommand ToCommand(
        this UpdateMainPetInfoRequest request,
        Guid volunteerId,
        Guid petId)
    {
        var coloration = Enum.TryParse(request.Coloration, true, out Colour resultColour)
            ? resultColour
            : Colour.Unknown;

        var appearanceDetails = new AppearanceDetailsDto(coloration, request.Weight, request.Height);

        var address = new AddressDto(request.Country, request.City, request.Street, request.PostalCode);

        var healthDetails = new HealthDetailsDto(request.HealthInformation, request.IsCastrated, request.IsVaccinated);

        var breedAndSpeciesId = new BreedAndSpeciesIdDto(request.SpeciesId, request.BreedId);

        return new UpdateMainPetInfoCommand(
            volunteerId,
            petId,
            request.Name,
            request.Description,
            appearanceDetails,
            address,
            request.PhoneNumber,
            request.BirthDate,
            healthDetails,
            request.Requisites,
            breedAndSpeciesId);
    }

    public static UpdatePetStatusCommand ToCommand(this UpdatePetStatusRequest request, Guid volunteerId, Guid petId)
    {
        var status = Enum.TryParse(request.Status, true, out Status statusResult)
            ? statusResult
            : Status.Unknown;

        return new UpdatePetStatusCommand(volunteerId, petId, status);
    }

    public static SetMainPetPhotoCommand ToCommand(this SetMainPetPhotoRequest request, Guid volunteerId, Guid petId)
    {
        return new SetMainPetPhotoCommand(volunteerId, petId, request.PhotoPath);
    }

    public static GetFilteredVolunteersWithPaginationQuery ToQuery(
        this GetFilteredVolunteersWithPaginationRequest request)
    {
        return new GetFilteredVolunteersWithPaginationQuery(
            request.PageNumber,
            request.PageSize,
            request.FirstName,
            request.LastName,
            request.Patronymic,
            request.WorkExperience,
            request.SortBy ?? "id",
            request.SortDirection ?? "ASC");
    }

    public static GetFilteredPetsWithPaginationQuery ToQuery(this GetFilteredPetsWithPaginationRequest request)
    {
        Colour? coloration = Enum.TryParse(request.Coloration, true, out Colour resultColour)
            ? resultColour
            : null;

        Status? status = Enum.TryParse(request.Status, true, out Status resultStatus)
            ? resultStatus
            : null;

        return new GetFilteredPetsWithPaginationQuery(
            request.PageNumber,
            request.PageSize,
            request.Name,
            coloration,
            request.Weight,
            request.Height,
            request.Country,
            request.City,
            request.Street,
            request.PostalCode,
            request.PhoneNumber,
            request.BirthDate,
            status,
            request.IsCastrated,
            request.IsVaccinated,
            request.Position,
            request.VolunteerId,
            request.BreedId,
            request.SpeciesId,
            request.SortBy ?? "id",
            request.SortDirection ?? "ASC");
    }
}