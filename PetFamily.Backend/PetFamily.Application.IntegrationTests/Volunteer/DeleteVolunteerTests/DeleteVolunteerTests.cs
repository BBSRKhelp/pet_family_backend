using System.Reflection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Delete;

namespace PetFamily.Application.IntegrationTests.Volunteer.DeleteVolunteerTests;

public class DeleteVolunteerTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
    
    public DeleteVolunteerTests(VolunteerTestsWebFactory factory) : base(factory)
    {
         _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task DeleteVolunteer_ShouldReturnGuid()
    {
        //Arrange
        var volunteerId = await SeedVolunteerAsync();
        
        var command = new DeleteVolunteerCommand(volunteerId);
        
        //Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().Be(volunteerId);
        
        var volunteer = await WriteDbContext.Volunteers.FirstOrDefaultAsync();
        
        var isDeleted = volunteer?
            .GetType()
            .GetField("_isDeleted", BindingFlags.NonPublic | BindingFlags.Instance)?
            .GetValue(volunteer);
        
        volunteer.Should().NotBeNull();
        isDeleted.Should().Be(true);
    }

    [Fact]
    public async Task DeleteVolunteer_WhenVolunteerDoesNotExist_ShouldReturnFailedResult()
    {
        //Arrange
        var command = new DeleteVolunteerCommand(Guid.NewGuid());
        
        //Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        
        var volunteer = await WriteDbContext.Volunteers.FirstOrDefaultAsync();
        
        volunteer.Should().BeNull();
    }
}