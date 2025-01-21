using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.SpeciesAggregate.Commands.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}