using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Species.Application.Features.Commands.Breed.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("Id"));
        
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}