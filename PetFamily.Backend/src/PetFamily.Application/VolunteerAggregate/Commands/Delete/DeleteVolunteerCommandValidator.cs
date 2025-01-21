using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
    }
}