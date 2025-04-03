using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateMainInfo;

namespace PetFamily.Application.IntegrationTests.Volunteer.UpdateMainInfoTests;

public class UpdateMainInfoTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateMainVolunteerInfoCommand> _sut;
    
    public UpdateMainInfoTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainVolunteerInfoCommand>>();
    }

    [Fact]
    public async Task UpdateMainInfo_ShouldUpdateVolunteer()
    {
        //Arrange
        var volunteerId = await SeedVolunteerAsync();

        var command = Fixture.BuildUpdateMainInfoVolunteerCommand(volunteerId);

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
    public async Task UpdateMainInfo_WhenVolunteerDoesNotExist_ShouldReturnFailedResults()
    {
        //Arrange
        var command = Fixture.BuildUpdateMainInfoVolunteerCommand(Guid.NewGuid());
        
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeEmpty();
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault();
        
        volunteer.Should().BeNull();
    }
}