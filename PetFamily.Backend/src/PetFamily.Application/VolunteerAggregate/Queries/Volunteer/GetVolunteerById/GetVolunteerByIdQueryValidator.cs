using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Core.Models;

namespace PetFamily.Application.VolunteerAggregate.Queries.Volunteer.GetVolunteerById;

public class GetVolunteerByIdQueryValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdQueryValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("volunteerId"));
    }
}