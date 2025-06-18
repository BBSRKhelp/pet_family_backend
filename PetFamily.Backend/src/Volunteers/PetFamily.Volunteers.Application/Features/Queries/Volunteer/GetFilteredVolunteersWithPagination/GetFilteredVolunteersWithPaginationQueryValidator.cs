using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationQueryValidator
    : AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadOnlyList<string> _allowedFilters = ["id"];

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
    }
}