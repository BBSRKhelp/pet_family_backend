using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateRequisites;

public class UpdateRequisitesVolunteerCommandValidator : AbstractValidator<UpdateRequisitesVolunteerCommand>
{
    public UpdateRequisitesVolunteerCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleForEach(u => u.Requisites)
            .MustBeValueObject(rd => Requisite.Create(rd.Title, rd.Description));
    }
}