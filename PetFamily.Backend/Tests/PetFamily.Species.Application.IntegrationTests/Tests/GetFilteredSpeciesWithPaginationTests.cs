using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Species.Application.Features.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.DTOs;

namespace PetFamily.Species.Application.IntegrationTests.Tests;

public class GetFilteredSpeciesWithPaginationTests : SpeciesTestsBase
{
    private readonly IQueryHandler<PagedList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery> _sut;

    public GetFilteredSpeciesWithPaginationTests(SpeciesTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<PagedList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery>>();
    }

    [Fact]
    public async Task GetFilteredSpeciesWithPagination_ShouldReturnSpeciesWithPagination()
    {
        //Arrange
        foreach (var _ in Enumerable.Range(0, 10))
            await SeedSpeciesAsync();
        
        var query = Fixture.BuildGetFilteredSpeciesWithPaginationQuery(pageNumber: 1, pageSize: 5);
        
        //Act
        var result = await _sut.HandleAsync(query);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(5);
        result.Value.TotalCount.Should().Be(10);
        result.Value.HasPreviousPage.Should().BeFalse();
        result.Value.HasNextPage.Should().BeTrue();

        var species = WriteDbContext.Species.ToList();
        
        species.Should().NotBeNull();
        species.Count.Should().Be(10);
    }
    
    [Fact]
    public async Task GetFilteredSpeciesWithPagination_WhenPaginationIsInvalid_ShouldReturnFailedResult()
    {
        //Arrange
        foreach (var _ in Enumerable.Range(0, 10))
            await SeedSpeciesAsync();
        
        var query = Fixture.BuildGetFilteredSpeciesWithPaginationQuery(pageNumber: -1, pageSize: -5);
        
        //Act
        var result = await _sut.HandleAsync(query);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}