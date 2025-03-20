using FluentValidation;
using PetFamily.Core.Models;
using PetFamily.Core.Validation;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
        
        RuleFor(x => x.BreedId).NotEmpty().WithError(Errors.General.IsRequired("BreedId"));
    }
}