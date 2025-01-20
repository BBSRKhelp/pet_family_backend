using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationQueryValidator
    : AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadOnlyList<string> _allowedFilters =
        ["id", "first_name", "last_name", "patronymic", "work_experience"];

    public GetFilteredVolunteersWithPaginationQueryValidator()
    {
        RuleFor(g => g.PageSize)
            .Must(i => i >= 1).WithError(Errors.General.MinLengthLowered("PageSize"));

        RuleFor(g => g.PageNumber)
            .Must(i => i >= 1).WithError(Errors.General.MinLengthLowered("PageNumber"));

        RuleFor(g => g.SortBy)
            .Must(sortBy => _allowedFilters.Contains(sortBy)).WithError(Errors.General.IsInvalid("SortBy"));

        RuleFor(g => g.SortDirection)
            .Must(sd => sd?.ToLower() is null or "asc" or "desc").WithError(Errors.General.IsInvalid("SortDirection"));

        RuleFor(g => g.FirstName)
            .MaximumLength(30).WithError(Errors.General.MaxLengthExceeded("FirstName"));

        RuleFor(g => g.LastName)
            .MaximumLength(30).WithError(Errors.General.MaxLengthExceeded("LastName"));

        RuleFor(g => g.Patronymic)
            .MaximumLength(30).WithError(Errors.General.MaxLengthExceeded("Patronymic"));

        RuleFor(g => g.WorkExperience)
            .Must(b => b is <= 100 or null).WithError(Errors.General.MaxLengthExceeded("WorkExperience"));
    }
}