using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Application.Features.Commands.Pet.UploadFilesToPet;

namespace PetFamily.Pet.Application.IntegrationTests.UploadFilesToPetTests;

public class UploadFilesToPetTests : PetTestsBase
{
    private readonly ICommandHandler<Guid, UploadFilesToPetCommand> _sut;
    
    public UploadFilesToPetTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UploadFilesToPetCommand>>();
    }

    [Fact]
    public async Task UploadFilesToPet_ShouldReturnGuid()
    {
        //Arrange
        Factory.SetupSuccessFileProviderMock();
        
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        using var stream = new MemoryStream(new byte[1]);
        var file = new UploadFileDto(stream, "test.file");
        
        var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, [file]);
            
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(pet.Id.Value);
        
        var petFromDb = VolunteerWriteDbContext
            .Volunteers.FirstOrDefault()?
            .Pets.FirstOrDefault();
        
        petFromDb?.PetPhotos.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task UploadFilesToPet_ShouldReturnFailedResult()
    {
        //Arrange
        Factory.SetupFailureFileProviderMock();
        
        var volunteer = await SeedVolunteerAsync();
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species);
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);
    
        using var stream = new MemoryStream(new byte[10]);
        var file = new UploadFileDto(stream, "test.file");
        
        var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, [file]);
            
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeEmpty();
    }
}