using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Features.Commands.Breed.DeleteBreed;

namespace PetFamily.Breed.Application.IntegrationTests.Tests;

public class DeleteBreedTests : BreedTestsBase
{
    private readonly ICommandHandler<Guid, DeleteBreedCommand> _sut;
    
    public DeleteBreedTests(BreedTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
    }

    [Fact]
    public async Task DeleteBreed_ShouldReturnGuid()
    {
        //Arrange
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species, "breedName");

        var command = new DeleteBreedCommand(species.Id.Value, breedId);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(breedId);
        
        var breed = WriteDbContext
            .Species.FirstOrDefault()?
            .Breeds.FirstOrDefault();
        
        breed.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteBreed_WhenSpeciesIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var species = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(species, "breedName");

        var command = new DeleteBreedCommand(Guid.Empty, breedId);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}