using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteer.Application.IntegrationTests.Tests;

public class GetFilteredVolunteersWithPaginationTests : VolunteerTestsBase
{
    private readonly IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery> _sut;

    public GetFilteredVolunteersWithPaginationTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<PagedList<VolunteerDto>,
            GetFilteredVolunteersWithPaginationQuery>>();
    }

    [Fact]
    public async Task GetFilteredVolunteersWithPagination_ShouldReturnVolunteers_WithPagination()
    {
        //Arrange
        foreach (var _ in Enumerable.Range(0, 11))
            await SeedVolunteerAsync();

        var query = Fixture.BuildGetFilteredVolunteersWithPaginationQuery(1, 2);
        
        //Act
        var result = await _sut.HandleAsync(query);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.TotalCount.Should().Be(11);
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(2);
        result.Value.HasNextPage.Should().BeTrue();
        result.Value.HasPreviousPage.Should().BeFalse();
        
        var volunteers = WriteDbContext.Volunteers.ToList();
        
        volunteers.Should().HaveCount(11);
    }

    [Fact]
    public async Task GetFilteredVolunteersWithPagination_WhenPaginationIsInvalid_ShouldReturnFailedResults()
    {
        //Arrange
        foreach (var _ in Enumerable.Range(0, 11))
            await SeedVolunteerAsync();

        var query = Fixture.BuildGetFilteredVolunteersWithPaginationQuery(-1, -2);
        
        //Act
        var result = await _sut.HandleAsync(query);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Count().Should().Be(2);
    }
}