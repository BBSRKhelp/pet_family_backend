using PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;

namespace PetFamily.API.Contracts.Volunteer;

public record GetVolunteersWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    byte? WorkExperience,
    string? SortDirection,
    string? SortBy)
    
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
            SortDirection ?? "ASC",
            SortBy ?? "id");
    }
}