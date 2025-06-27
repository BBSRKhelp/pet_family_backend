using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Features.Commands.Breed.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
        
        RuleFor(x => x.BreedId).NotEmpty().WithError(Errors.General.IsRequired("BreedId"));
    }
}