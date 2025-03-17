using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Commands.Species.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
    }
}