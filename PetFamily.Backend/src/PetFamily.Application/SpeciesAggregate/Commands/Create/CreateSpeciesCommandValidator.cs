using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.SpeciesAggregate.Commands.Create;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}