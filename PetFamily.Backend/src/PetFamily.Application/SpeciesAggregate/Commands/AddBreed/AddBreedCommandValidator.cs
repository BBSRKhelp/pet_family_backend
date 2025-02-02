using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.SpeciesAggregate.Commands.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("Id"));
        
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}