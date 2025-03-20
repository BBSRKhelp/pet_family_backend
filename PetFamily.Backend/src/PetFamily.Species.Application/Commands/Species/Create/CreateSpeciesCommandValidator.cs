using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Species.Application.Commands.Species.Create;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}