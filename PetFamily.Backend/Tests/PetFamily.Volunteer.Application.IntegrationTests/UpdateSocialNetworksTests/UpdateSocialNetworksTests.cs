using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateSocialNetworks;

namespace PetFamily.Volunteer.Application.IntegrationTests.UpdateSocialNetworksTests;

public class UpdateSocialNetworksTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateSocialNetworksVolunteerCommand> _sut;
    
    public UpdateSocialNetworksTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksVolunteerCommand>>();
    }

    [Fact]
    public async Task UpdateSocialNetworks_ShouldUpdateSocialNetworks()
    {
        //Arrange
        var volunteerId = await SeedVolunteerAsync();

        var command = Fixture.BuildUpdateSocialNetworksVolunteerCommand(volunteerId);

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(volunteerId);
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault();
        
        volunteer.Should().NotBeNull();
    }
    
    [Fact]
    public async Task UpdateSocialNetworks_WhenVolunteerDoesNotExist_ShouldReturnFailedResults()
    {
        //Arrange
        var command = Fixture.BuildUpdateSocialNetworksVolunteerCommand(Guid.NewGuid());

        //Act
        var result = await _sut.HandleAsync(command);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeEmpty();
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault();
        
        volunteer.Should().BeNull();
    }
}