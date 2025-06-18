using FluentValidation;
using PetFamily.Core.Enums;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.UpdateMainPetInfo;

public class UpdateMainPetInfoCommandValidator : AbstractValidator<UpdateMainPetInfoCommand>
{
    public UpdateMainPetInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.IsRequired("PetId"));

        RuleFor(u => u.Name).MustBeValueObject(Name.Create);

        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        
        RuleFor(u => u.AppearanceDetails.Coloration)
            .IsInEnum()
            .Must(x => x != Colour.Unknown)
            .WithError(Errors.General.IsInvalid("Coloration"));

        RuleFor(u => u.AppearanceDetails)
            .Must(x => x.Coloration != Colour.Unknown)
            .MustBeValueObject(x => AppearanceDetails.Create(x.Coloration, x.Weight, x.Height));

        RuleFor(u => u.Address)
            .MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.PostalCode));

        RuleFor(u => u.BreedAndSpeciesId)
            .MustBeValueObject(x => BreedAndSpeciesId.Create(x.SpeciesId, x.BreedId));
    }
}