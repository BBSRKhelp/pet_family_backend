using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Application.Features.Queries.Volunteer.GetVolunteerById;
using PetFamily.Volunteer.Contracts.DTOs;

namespace PetFamily.Application.IntegrationTests.Volunteer.GetVolunteerByIdTests;

public class GetVolunteerByIdTests : VolunteerTestsBase
{
    private readonly IQueryHandler<VolunteerDto, GetVolunteerByIdQuery> _sut;
    
    public GetVolunteerByIdTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>>();
    }

    [Fact]
    public async Task GetVolunteerById_ShouldReturnsVolunteer()
    {
        //Arrange
        var volunteerId = Guid.Empty;
        foreach (var _ in Enumerable.Range(0, 5))
            volunteerId = await SeedVolunteerAsync();
        
        var command = new GetVolunteerByIdQuery(volunteerId);
        
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(volunteerId);
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault(v => v.Id == volunteerId);
        
        volunteer.Should().NotBeNull();
    }

    [Fact]
    public async Task GetVolunteerById_WhenVolunteerIdIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        var volunteerId = Guid.Empty;
        
        foreach (var _ in Enumerable.Range(0, 5))
            await SeedVolunteerAsync();
        
        var command = new GetVolunteerByIdQuery(volunteerId);
        
        //Act
        var result = await _sut.HandleAsync(command);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}