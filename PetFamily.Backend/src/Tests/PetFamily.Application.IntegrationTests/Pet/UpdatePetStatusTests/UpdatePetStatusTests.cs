using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdatePetStatus;

namespace PetFamily.Application.IntegrationTests.Pet.UpdatePetStatusTests;

public class UpdatePetStatusTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, UpdatePetStatusCommand> _sut;

    public UpdatePetStatusTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetStatusCommand>>();
    }

    [Fact]
    public async Task UpdatePetStatus_ShouldReturnGuid()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        const Status STATUS = Status.UndergoingTreatment;

        var command = new UpdatePetStatusCommand(
            volunteer.Id.Value,
            pet.Id.Value,
            STATUS);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Id.Value);

        var petFromDb = VolunteerWriteDbContext
            .Volunteers.FirstOrDefault()?
            .Pets.FirstOrDefault();

        petFromDb.Should().NotBeNull();
        petFromDb.Status.Should().Be(STATUS);
    }

    [Fact]
    public async Task UpdatePetStatus_WhenVolunteerIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        const Status STATUS = Status.UndergoingTreatment;

        var command = new UpdatePetStatusCommand(
            Guid.Empty,
            pet.Id.Value,
            STATUS);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}