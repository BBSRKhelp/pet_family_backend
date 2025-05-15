using AutoFixture;
using PetFamily.Volunteers.Application.Features.Commands.Volunteer.Create;
using PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateRequisites;
using PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Application.IntegrationTests;

public static class VolunteerFixtureExtensions
{
    public static CreateVolunteerCommand BuildCreateVolunteerCommand(this Fixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>()
            .With(v => v.FullName, new FullNameDto("testname", "testlastname", "testpatronymic"))
            .With(v => v.Email, "test@test.com")
            .With(v => v.PhoneNumber, "89123456789")
            .With(v => v.WorkExperience, 99)
            .Create();
    }

    public static UpdateMainVolunteerInfoCommand BuildUpdateMainInfoVolunteerCommand(
        this Fixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateMainVolunteerInfoCommand>()
            .With(v => v.Id, volunteerId)
            .With(v => v.FullName, new FullNameDto("testname2", "testlastname2", "testpatronymic2"))
            .With(v => v.Email, "test2@test2.com2")
            .With(v => v.PhoneNumber, "89123456788")
            .With(v => v.WorkExperience, 98)
            .Create();
    }

    public static UpdateRequisitesVolunteerCommand BuildUpdateRequisitesVolunteerCommand(
        this Fixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateRequisitesVolunteerCommand>()
            .With(v => v.Id, volunteerId)
            .With(v => v.Requisites)
            .Create();
    }
    
    public static UpdateSocialNetworksVolunteerCommand BuildUpdateSocialNetworksVolunteerCommand(
        this Fixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateSocialNetworksVolunteerCommand>()
            .With(v => v.Id, volunteerId)
            .With(v => v.SocialNetworks)
            .Create();
    }

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
            .With(v => v.WorkExperience, (byte?)null)
            .With(v => v.FirstName, (string?)null)
            .With(v => v.LastName, (string?)null)
            .With(v => v.Patronymic, (string?)null)
            .Create();
    }
}