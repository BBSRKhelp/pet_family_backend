using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Commands.Species.AddBreed;

public class BreedCreateCommandValidator : AbstractValidator<BreedCreateCommand>
{
    public BreedCreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}