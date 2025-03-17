using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Core.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
    }
}