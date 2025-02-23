using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.DTOs.Read;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Models;
using PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWithPagination;

namespace PetFamily.Application.IntegrationTests.Species.GetFilteredSpeciesWithPaginationTests;

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