using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.VolunteerAggregate.Commands.Pet.HardDeletePet;

namespace PetFamily.Application.IntegrationTests.Pet.HardDeletePetTests;

public class HardDeletePetTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, HardDeletePetCommand> _sut;

    public HardDeletePetTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, HardDeletePetCommand>>();
    }

    [Fact]
    public async Task HardDeletePet_ShouldReturnGuid()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new HardDeletePetCommand(volunteer.Id.Value, pet.Id.Value);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.GetType().Should().Be(typeof(Guid));
        
        var pets = WriteDbContext.Volunteers.FirstOrDefault()?.Pets;
        
        pets.Should().BeEmpty();
        pets.Count.Should().Be(0);
    }

    [Fact]
    public async Task HardDeletePet_WhenVolunteerIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new HardDeletePetCommand(Guid.Empty, pet.Id.Value);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}