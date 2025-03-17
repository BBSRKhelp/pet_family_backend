using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Core.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SoftDeletePet;

public class SoftDeletePetCommandValidator : AbstractValidator<SoftDeletePetCommand>
{
    public SoftDeletePetCommandValidator()
    {
        RuleFor(sd => sd.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(sd => sd.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
    }
}