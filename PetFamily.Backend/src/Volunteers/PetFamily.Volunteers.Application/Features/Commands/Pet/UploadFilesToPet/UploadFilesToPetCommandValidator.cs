using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.UploadFilesToPet;

public class UploadFilesToPetCommandValidator : AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired(nameof(VolunteerId)));

        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.IsRequired(nameof(PetId)));

        RuleForEach(u => u.Files)
            .Must(x => x.Stream.Length < 50000000)
            .MustBeValueObject(x => PhotoPath.Create(x.FileName));
    }
}