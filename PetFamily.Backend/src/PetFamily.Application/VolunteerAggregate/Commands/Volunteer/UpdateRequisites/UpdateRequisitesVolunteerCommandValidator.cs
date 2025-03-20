using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Core.Models;
using PetFamily.Core.Validation;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateRequisites;

public class UpdateRequisitesVolunteerCommandValidator : AbstractValidator<UpdateRequisitesVolunteerCommand>
{
    public UpdateRequisitesVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.Requisites)
            .MustBeValueObject(rd => Requisite.Create(rd.Title, rd.Description));
    }
}