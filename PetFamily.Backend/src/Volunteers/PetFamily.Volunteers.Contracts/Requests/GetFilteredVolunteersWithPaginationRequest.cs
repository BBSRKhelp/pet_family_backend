
namespace PetFamily.Volunteers.Contracts.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    int PageNumber,
    int PageSize,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    byte? WorkExperience,
    string? SortBy,
    string? SortDirection);