using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.HardDeletePet;

public class HardDeletePetCommandValidator : AbstractValidator<HardDeletePetCommand>
{
    public HardDeletePetCommandValidator()
    {
        RuleFor(sd => sd.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(sd => sd.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
    }
}