using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Features.Commands.Pet.SoftDeletePet;

namespace PetFamily.Pet.Application.IntegrationTests.SoftDeletePetTests;

public class SoftDeletePetTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, SoftDeletePetCommand> _sut;

    public SoftDeletePetTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeletePetCommand>>();
    }

    [Fact]
    public async Task SoftDeletePet_ShouldReturnGuid()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new SoftDeletePetCommand(volunteer.Id.Value, pet.Id.Value);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(pet.Id.Value);

        var petFromDb = VolunteerWriteDbContext
            .Volunteers.FirstOrDefault()
            ?.Pets.FirstOrDefault();

        var isDeleted = petFromDb?
            .GetType()
            .GetField("_isDeleted", BindingFlags.NonPublic | BindingFlags.Instance)?
            .GetValue(pet);

        petFromDb.Should().NotBeNull();
        isDeleted.Should().Be(true);
    }

    [Fact]
    public async Task SoftDeletePet_WhenVolunteerIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new SoftDeletePetCommand(Guid.Empty, pet.Id.Value);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}