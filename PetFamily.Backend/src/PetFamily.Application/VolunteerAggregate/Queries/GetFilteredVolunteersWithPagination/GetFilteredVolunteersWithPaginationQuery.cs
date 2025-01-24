using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    int PageNumber,
    int PageSize,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    byte? WorkExperience, 
    string SortBy,
    string SortDirection) : IQuery;
