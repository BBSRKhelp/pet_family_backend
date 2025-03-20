using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Queries.Volunteer.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    byte? WorkExperience, 
    string SortBy,
    string SortDirection) : IQuery;
