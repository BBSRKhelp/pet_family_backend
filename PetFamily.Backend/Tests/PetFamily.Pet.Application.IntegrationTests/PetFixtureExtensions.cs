using AutoFixture;
using PetFamily.Core.Enums;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Application.Features.Commands.Pet.AddPet;
using PetFamily.Volunteers.Application.Features.Commands.Pet.ChangePetsPosition;
using PetFamily.Volunteers.Application.Features.Commands.Pet.UpdateMainPetInfo;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Pet.Application.IntegrationTests;

public static class PetFixtureExtensions
{
    public static AddPetCommand BuildAddPetCommand(
        this Fixture fixture,
        Guid volunteerId,
        SpeciesId speciesId,
        Guid breedId)
    {
        fixture.Customize(new DateOnlyCustomization());
        
        return fixture.Build<AddPetCommand>()
            .With(p => p.BirthDate, DateOnly.Parse("2023-12-01"))
            .With(p => p.VolunteerId, volunteerId)
            .With(p => p.Name, (string?)null)
            .With(p => p.Description, (string?)null)
            .With(p => p.AppearanceDetails, new AppearanceDetailsDto(Colour.Black, 12, 12))
            .With(p => p.HealthDetails, new HealthDetailsDto("test", true, true))
            .With(p => p.Address, new AddressDto("co", "ci", "st", null))
            .With(p => p.BreedAndSpeciesId, new BreedAndSpeciesIdDto(speciesId, breedId))
            .With(p => p.Status, Status.FoundHome)
            .Create();
    }

    public static ChangePetsPositionCommand BuildChangePetsPositionCommand(
        this Fixture fixture,
        Guid volunteerId,
        Guid petId,
        int newPosition)
    {
        return fixture.Build<ChangePetsPositionCommand>()
            .With(p => p.VolunteerId, volunteerId)
            .With(p => p.PetId, petId)
            .With(p => p.NewPosition, newPosition)
            .Create();
    }

    public static UpdateMainPetInfoCommand BuildUpdateMainPetInfoCommand(
        this Fixture fixture,
        Guid volunteerId,
        Guid petId,
        SpeciesId speciesId,
        Guid breedId)
    {
        fixture.Customize(new DateOnlyCustomization());
        
        return fixture.Build<UpdateMainPetInfoCommand>()
            .With(p => p.VolunteerId, volunteerId)
            .With(p => p.PetId, petId)
            .With(p => p.AppearanceDetails, new AppearanceDetailsDto(Colour.Black, 12, 12))
            .With(p => p.Address, new AddressDto("co", "ci", "st", null))
            .With(p => p.BirthDate, DateOnly.Parse("2023-12-01"))
            .With(p => p.HealthDetails, new HealthDetailsDto("test", true, true))
            .With(p => p.BreedAndSpeciesId, new BreedAndSpeciesIdDto(speciesId, breedId))
            .Create();
    }
}

internal class DateOnlyCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => DateOnly.FromDateTime(fixture.Create<DateTime>()));
    }
}