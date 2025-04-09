using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.SetMainPetPhoto;

public class SetMainPetPhotoCommandValidator : AbstractValidator<SetMainPetPhotoCommand>
{
    public SetMainPetPhotoCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
        
        RuleFor(s => s.PhotoPath).MustBeValueObject(PhotoPath.Create);
    }
}