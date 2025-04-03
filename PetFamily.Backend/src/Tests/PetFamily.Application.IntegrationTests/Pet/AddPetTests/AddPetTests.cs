using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;

namespace PetFamily.Application.IntegrationTests.Pet.AddPetTests;

public class AddPetTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, AddPetCommand> _sut;
    
    public AddPetTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
    }

    [Fact]
    public async Task AddPet_ShouldReturnGuid()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        
        var command = Fixture.BuildAddPetCommand(volunteer.Id.Value, species.Id, breedId);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var pet = WriteDbContext
            .Volunteers.FirstOrDefault()
            ?.Pets.FirstOrDefault();
        
        pet.Should().NotBeNull();
        pet.Id.Value.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task AddPet_WhenEntitiesDoesntExist_ShouldReturnFailedResult()
    {
        //Arrange
        var command = Fixture.BuildAddPetCommand(Guid.Empty, SpeciesId.Empty(), Guid.Empty);
        
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}