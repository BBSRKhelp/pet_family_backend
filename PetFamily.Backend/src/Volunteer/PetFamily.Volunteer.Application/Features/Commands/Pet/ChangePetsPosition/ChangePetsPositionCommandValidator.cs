using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteer.Domain.ValueObjects;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.ChangePetsPosition;

public class ChangePetsPositionCommandValidator : AbstractValidator<ChangePetsPositionCommand>
{
    public ChangePetsPositionCommandValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));

        RuleFor(c => c.NewPosition).MustBeValueObject(Position.Create);
    }
}