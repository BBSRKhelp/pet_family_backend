using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.SpeciesAggregate.Commands.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(Errors.General.IsRequired("SpeciesId"));
    }
}