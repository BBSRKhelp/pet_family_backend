using AutoFixture;
using PetFamily.Species.Application.Features.Queries.GetFilteredSpeciesWithPagination;

namespace PetFamily.Species.Application.IntegrationTests;

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