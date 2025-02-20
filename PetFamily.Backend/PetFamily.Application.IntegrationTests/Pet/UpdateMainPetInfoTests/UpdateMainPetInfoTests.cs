using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.VolunteerAggregate.Commands.Pet.UpdateMainPetInfo;

namespace PetFamily.Application.IntegrationTests.Pet.UpdateMainPetInfoTests;

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
        
        const string PHONE_NUMBER = "89999990000";
        
        var command = Fixture.BuildUpdateMainPetInfoCommand(
            volunteer.Id.Value,
            pet.Id.Value, 
            species.Id,
            breedId,
            PHONE_NUMBER);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(pet.Id.Value);

        var petFromDb = WriteDbContext
            .Volunteers.FirstOrDefault()?
            .Pets.FirstOrDefault();
        
        petFromDb.Should().NotBeNull();
        petFromDb.PhoneNumber.Value.Should().Be(PHONE_NUMBER);
    }

    [Fact]
    public async Task UpdateMainPetPhoto_WhenPhoneNumberIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);
        
        const string PHONE_NUMBER = "989999990000";
        
        var command = Fixture.BuildUpdateMainPetInfoCommand(
            volunteer.Id.Value,
            pet.Id.Value, 
            species.Id,
            breedId,
            PHONE_NUMBER);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}