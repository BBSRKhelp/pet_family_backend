using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryValidator : AbstractValidator<GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadOnlyList<string> _allowedFilters =
    [
        "id",
        "name",
        "coloration",
        "weight",
        "height",
        "country",
        "city",
        "street",
        "postal_code",
        "birth_date",
        "status",
        "is_castrated",
        "is_vaccinated",
        "position",
        "volunteer_id",
        "species_id",
        "breed_id"
    ];

    public GetFilteredPetsWithPaginationQueryValidator()
    {
        RuleFor(g => g.PageSize)
            .Must(i => i >= 1).WithError(Errors.General.MinLengthLowered("PageSize"));

        RuleFor(g => g.PageNumber)
            .Must(i => i >= 1).WithError(Errors.General.MinLengthLowered("PageNumber"));

        RuleFor(g => g.SortBy)
            .Must(sortBy => _allowedFilters.Contains(sortBy)).WithError(Errors.General.IsInvalid("SortBy"));

        RuleFor(g => g.SortDirection)
            .Must(sd => sd?.ToLower() is null or "asc" or "desc").WithError(Errors.General.IsInvalid("SortDirection"));

        RuleFor(g => g.Name).MustBeValueObject(Name.Create);

        RuleFor(g => g.Coloration).IsInEnum().WithError(Errors.General.IsInvalid("Coloration"));

        RuleFor(g => g.Weight).Must(x => x is < 1000 or null).WithError(Errors.General.MaxLengthExceeded("Weight"));

        RuleFor(g => g.Height).Must(x => x is < 1000 or null).WithError(Errors.General.MaxLengthExceeded("Height"));

        RuleFor(g => g.BirthDate).Must(x => x?.Year is > 1990 or null)
            .WithError(Errors.General.MaxLengthExceeded("BirthDate"));

        RuleFor(g => g.Status).IsInEnum().WithError(Errors.General.IsInvalid("Status"));

        RuleFor(g => g.Position).Must(x => x is > 0 or null).WithError(Errors.General.MinLengthLowered("Position"));
    }
}