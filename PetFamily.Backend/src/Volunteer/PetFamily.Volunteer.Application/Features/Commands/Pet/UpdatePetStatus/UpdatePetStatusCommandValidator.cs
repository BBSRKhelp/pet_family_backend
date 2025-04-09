using FluentValidation;
using PetFamily.Core.Enums;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
        
        RuleFor(u => u.Status)
            .IsInEnum()
            .Must(x => x != Status.Unknown)
            .WithError(Errors.General.IsInvalid("Status"));
    }
}