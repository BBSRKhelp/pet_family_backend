using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public class VolunteerUpdateRequisitesCommandValidator : AbstractValidator<VolunteerUpdateRequisitesCommand>
{
    public VolunteerUpdateRequisitesCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.Requisites)
            .MustBeValueObject(rd => Requisite.Create(rd.Title, rd.Description));
    }
}