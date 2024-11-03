using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.Commands.Volunteer.Delete;

public class VolunteerDeleteCommandValidator : AbstractValidator<VolunteerDeleteCommand>
{
    public VolunteerDeleteCommandValidator()
    {
        RuleFor(d => d.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
    }
}