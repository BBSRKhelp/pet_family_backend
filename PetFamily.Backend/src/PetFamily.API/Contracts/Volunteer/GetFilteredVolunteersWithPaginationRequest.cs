using PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;

namespace PetFamily.API.Contracts.Volunteer;

public record GetFilteredVolunteersWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    byte? WorkExperience,
    string? SortBy,
    string? SortDirection)
    
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery()
    {
        return new GetFilteredVolunteersWithPaginationQuery(
            PageNumber,
            PageSize,
            FirstName,
            LastName,
            Patronymic,
            WorkExperience,
            SortBy ?? "id",
            SortDirection ?? "ASC");
    }
}