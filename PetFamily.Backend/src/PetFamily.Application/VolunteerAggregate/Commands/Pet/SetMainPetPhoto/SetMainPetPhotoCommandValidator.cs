using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SetMainPetPhoto;

public class SetMainPetPhotoCommandValidator : AbstractValidator<SetMainPetPhotoCommand>
{
    public SetMainPetPhotoCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
        
        RuleFor(s => s.PhotoPath).MustBeValueObject(PhotoPath.Create);
    }
}