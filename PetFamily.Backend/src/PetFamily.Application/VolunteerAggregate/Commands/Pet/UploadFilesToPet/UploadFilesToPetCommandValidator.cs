using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.UploadFilesToPet;

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