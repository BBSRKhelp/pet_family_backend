using FluentValidation;
using PetFamily.Core.Enums;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired("VolunteerId"));

        RuleFor(c => c.Name).MustBeValueObject(Name.Create);

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.AppearanceDetails.Coloration)
            .IsInEnum()
            .Must(x => x != Colour.Unknown)
            .WithError(Errors.General.IsInvalid("Coloration"));

        RuleFor(c => c.AppearanceDetails)
            .Must(x => x.Coloration != Colour.Unknown)
            .MustBeValueObject(x => AppearanceDetails.Create(x.Coloration, x.Weight, x.Height));

        RuleFor(c => c.HealthDetails)
            .MustBeValueObject(x => HealthDetails.Create(x.HealthInformation, x.IsCastrated, x.IsVaccinated));

        RuleFor(c => c.Address)
            .MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.PostalCode));

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(c => c.Status).IsInEnum()
            .Must(x => x != Status.Unknown)
            .WithError(Errors.General.IsInvalid("Status"));

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

        RuleFor(c => c.BreedAndSpeciesId)
            .MustBeValueObject(x => BreedAndSpeciesId.Create(x.SpeciesId, x.BreedId));
    }
}