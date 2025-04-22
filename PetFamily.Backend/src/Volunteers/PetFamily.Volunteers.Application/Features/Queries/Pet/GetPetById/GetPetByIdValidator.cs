using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Features.Queries.Pet.GetPetById;

public class GetPetByIdValidator : AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdValidator()
    {
        RuleFor(g => g.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));
    }
}