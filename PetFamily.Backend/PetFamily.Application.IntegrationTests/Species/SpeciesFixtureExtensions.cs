using AutoFixture;
using PetFamily.Application.SpeciesAggregate.Queries.GetFilteredSpeciesWithPagination;

namespace PetFamily.Application.IntegrationTests.Species;

public static class SpeciesFixtureExtensions
{
    public static GetFilteredSpeciesWithPaginationQuery BuildGetFilteredSpeciesWithPaginationQuery(
        this Fixture fixture,
        int pageNumber,
        int pageSize)
    {
        return fixture.Build<GetFilteredSpeciesWithPaginationQuery>()
            .With(s => s.PageNumber, pageNumber)
            .With(s => s.PageSize, pageSize)
            .With(s => s.Name, (string?)null)
            .With(s => s.SortBy, "id")
            .With(s => s.SortDirection, "ASC")
            .Create();
    }
}