using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Features.Commands.Species.Create;

namespace PetFamily.Species.Application.IntegrationTests.Tests;

public class CreateSpeciesTests : SpeciesTestsBase
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;
    
    public CreateSpeciesTests(SpeciesTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
    }

    [Fact]
    public async Task CreateSpecies_ShouldReturnGuid()
    {
        //Arrange
        var command = new CreateSpeciesCommand("nameSpecies");
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var species = WriteDbContext.Species.FirstOrDefault();
        
        species.Should().NotBeNull();
        species.Id.Value.Should().Be(result.Value);
    }
    
    [Fact]
    public async Task CreateSpecies_WhenNameIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var command = new CreateSpeciesCommand(new string('a', 51));
        
        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}