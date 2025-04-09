using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.SoftDeletePet;

public class SoftDeletePetCommandValidator : AbstractValidator<SoftDeletePetCommand>
{
    public SoftDeletePetCommandValidator()
    {
        RuleFor(sd => sd.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(sd => sd.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
    }
}