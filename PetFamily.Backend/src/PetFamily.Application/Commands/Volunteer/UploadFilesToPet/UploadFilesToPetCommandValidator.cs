using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

namespace PetFamily.Application.Commands.Volunteer.UploadFilesToPet;

public class UploadFilesToPetCommandValidator : AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired(nameof(VolunteerId)));

        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.IsRequired(nameof(PetId)));

        RuleForEach(u => u.Files)
            .Must(x => x.Stream.Length < 5000000)
            .MustBeValueObject(x => PhotoPath.Create(Path.GetExtension(x.FileName)));
    }
}