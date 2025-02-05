using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateMainInfo;

public class UpdateMainInfoVolunteerCommandValidator : AbstractValidator<UpdateMainInfoVolunteerCommand>
{
    public UpdateMainInfoVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));
        
        RuleFor(u => u.FullName)
            .MustBeValueObject(fnd => FullName.Create(fnd.FirstName, fnd.LastName, fnd.Patronymic));
        
        RuleFor(u => u.Email).MustBeValueObject(Email.Create);

        RuleFor(u => u.Description).MustBeValueObject(Description.Create);

        RuleFor(u => u.WorkExperience).MustBeValueObject(WorkExperience.Create);

        RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}