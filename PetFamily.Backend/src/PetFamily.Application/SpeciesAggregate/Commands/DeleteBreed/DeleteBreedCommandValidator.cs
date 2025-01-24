using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
        
        RuleFor(x => x.BreedId).NotEmpty().WithError(Errors.General.IsRequired("BreedId"));
    }
}