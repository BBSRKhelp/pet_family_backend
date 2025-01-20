using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.UpdateRequisites;

public class UpdateRequisitesVolunteerCommandValidator : AbstractValidator<UpdateRequisitesVolunteerCommand>
{
    public UpdateRequisitesVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.Requisites)
            .MustBeValueObject(rd => Requisite.Create(rd.Title, rd.Description));
    }
}