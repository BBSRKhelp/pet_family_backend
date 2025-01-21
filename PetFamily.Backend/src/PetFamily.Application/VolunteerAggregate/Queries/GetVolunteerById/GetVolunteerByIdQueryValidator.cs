using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Queries.GetVolunteerById;

public class GetVolunteerByIdQueryValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdQueryValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("volunteerId"));
    }
}