using FluentValidation;
using PetFamily.Core.Models;
using PetFamily.Core.Validation;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Species.Application.Commands.Breed.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("Id"));
        
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}