using FluentValidation;
using PetFamily.Core.Enums;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteer.Domain.ValueObjects;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.UpdateMainPetInfo;

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
            .MustBeValueObject(x => AppearanceDetails.Create(x.Coloration, x.Weight, x.Height));

        RuleFor(u => u.Address)
            .MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.PostalCode));

        RuleFor(u => u.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(u => u.HealthDetails)
            .MustBeValueObject(x => HealthDetails.Create(x.HealthInformation, x.IsCastrated, x.IsVaccinated));

        RuleForEach(u => u.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

        RuleFor(u => u.BreedAndSpeciesId)
            .MustBeValueObject(x => BreedAndSpeciesId.Create(x.SpeciesId, x.BreedId));
    }
}