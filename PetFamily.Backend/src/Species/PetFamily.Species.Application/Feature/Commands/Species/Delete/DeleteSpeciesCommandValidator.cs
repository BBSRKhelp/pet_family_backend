using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Feature.Commands.Species.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
    }
}