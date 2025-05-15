using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateMainInfo;

public class UpdateMainVolunteerInfoCommandValidator : AbstractValidator<UpdateMainVolunteerInfoCommand>
{
    public UpdateMainVolunteerInfoCommandValidator()
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