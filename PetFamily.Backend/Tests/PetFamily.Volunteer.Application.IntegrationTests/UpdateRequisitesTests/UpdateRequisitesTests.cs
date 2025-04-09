using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateRequisites;

namespace PetFamily.Volunteer.Application.IntegrationTests.UpdateRequisitesTests;

public class UpdateRequisitesTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateRequisitesVolunteerCommand> _sut;
    
    public UpdateRequisitesTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateRequisitesVolunteerCommand>>();
    }

    [Fact]
    public async Task UpdateRequisites_ShouldUpdateRequisites()
    {
        //Arrange
        var volunteerId = await SeedVolunteerAsync();

        var command = Fixture.BuildUpdateRequisitesVolunteerCommand(volunteerId);

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
    public async Task UpdateRequisites_WhenVolunteerDoesNotExist_ShouldReturnFailedResults()
    {
        //Arrange
        var command = Fixture.BuildUpdateRequisitesVolunteerCommand(Guid.NewGuid());
        
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeEmpty();
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault();
        
        volunteer.Should().BeNull();
    }
}