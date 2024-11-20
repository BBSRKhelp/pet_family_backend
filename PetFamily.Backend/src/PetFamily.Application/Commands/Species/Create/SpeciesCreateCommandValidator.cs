using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Commands.Species.Create;

public class SpeciesCreateCommandValidator : AbstractValidator<SpeciesCreateCommand>
{
    public SpeciesCreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}