using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Feature.Commands.Species.Delete;

namespace PetFamily.Species.Application.IntegrationTests.Tests;

public class DeleteSpeciesTests : SpeciesTestsBase
{
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;
    
    public DeleteSpeciesTests(SpeciesTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
    }

    [Fact]
    public async Task DeleteSpecies_ShouldReturnGuid()
    {
        //Arrange
        var speciesId = await SeedSpeciesAsync();
        
        var command = new DeleteSpeciesCommand(speciesId);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(speciesId);
        
        var species = WriteDbContext.Species.FirstOrDefault();
        
        species.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteSpecies_WhenSpeciesIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        await SeedSpeciesAsync();
        
        var command = new DeleteSpeciesCommand(Guid.Empty);
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}