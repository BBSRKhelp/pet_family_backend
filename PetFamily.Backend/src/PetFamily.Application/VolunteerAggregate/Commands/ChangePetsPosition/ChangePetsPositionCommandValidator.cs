using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.ChangePetsPosition;

public class ChangePetsPositionCommandValidator : AbstractValidator<ChangePetsPositionCommand>
{
    public ChangePetsPositionCommandValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));

        RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
    }
}