using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Features.Commands.Pet.UpdateMainPetInfo;

namespace PetFamily.Pet.Application.IntegrationTests.Tests;

public class UpdateMainPetInfoTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, UpdateMainPetInfoCommand> _sut;
    
    public UpdateMainPetInfoTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainPetInfoCommand>>();
    }

    [Fact]
    public async Task UpdateMainPetPhoto_ShouldReturnGuid()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);
        
        var command = Fixture.BuildUpdateMainPetInfoCommand(
            volunteer.Id.Value,
            pet.Id.Value, 
            species.Id,
            breedId);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(pet.Id.Value);

        var petFromDb = VolunteerWriteDbContext
            .Volunteers.FirstOrDefault()?
            .Pets.FirstOrDefault();
        
        petFromDb.Should().NotBeNull();
    }
}