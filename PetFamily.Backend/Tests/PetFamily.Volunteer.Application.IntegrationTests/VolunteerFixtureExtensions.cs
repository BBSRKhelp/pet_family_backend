using AutoFixture;
using PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;

namespace PetFamily.Volunteer.Application.IntegrationTests;

public static class VolunteerFixtureExtensions
{
    public static GetFilteredVolunteersWithPaginationQuery BuildGetFilteredVolunteersWithPaginationQuery(
        this Fixture fixture,
        int page,
        int pageSize)
    {
        return fixture.Build<GetFilteredVolunteersWithPaginationQuery>()
            .With(v => v.PageNumber, page)
            .With(v => v.PageSize, pageSize)
            .With(v => v.SortBy, "id")
            .With(v => v.SortDirection, "ASC")
            .Create();
    }
}