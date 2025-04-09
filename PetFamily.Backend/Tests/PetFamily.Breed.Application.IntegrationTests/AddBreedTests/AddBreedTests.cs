using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Feature.Commands.Breed.AddBreed;

namespace PetFamily.Breed.Application.IntegrationTests.AddBreedTests;

public class AddBreedTests : BreedTestsBase
{
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;

    public AddBreedTests(BreedTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }

    [Fact]
    public async Task AddBreed_ShouldReturnGuid()
    {
        //Arrange
        var species = await SeedSpeciesAsync();

        var command = new AddBreedCommand(species.Id.Value, "nameBreed");

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var breed = WriteDbContext
            .Species.FirstOrDefault()?
            .Breeds.FirstOrDefault();

        breed.Should().NotBeNull();
        breed.Id.Value.Should().Be(result.Value);
    }

    [Fact]
    public async Task AddBreed_WhenSpeciesIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        await SeedSpeciesAsync();

        var command = new AddBreedCommand(Guid.Empty, "nameBreed");

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();

        var breed = WriteDbContext
            .Species.FirstOrDefault()?
            .Breeds.FirstOrDefault();

        breed.Should().BeNull();
    }
}