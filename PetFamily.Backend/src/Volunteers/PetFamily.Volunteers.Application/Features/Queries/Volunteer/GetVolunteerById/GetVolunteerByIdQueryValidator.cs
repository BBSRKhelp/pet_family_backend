using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetVolunteerById;

public class GetVolunteerByIdQueryValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdQueryValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("volunteerId"));
    }
}