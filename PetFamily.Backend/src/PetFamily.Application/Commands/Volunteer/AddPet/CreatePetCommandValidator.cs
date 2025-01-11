using FluentValidation;
using PetFamily.Application.Validators;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Commands.Volunteer.AddPet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.AppearanceDetails)
            .MustBeValueObject(x => AppearanceDetails.Create(x.Colouration, x.Weight, x.Height));

        RuleFor(c => c.HealthDetails)
            .MustBeValueObject(x => HealthDetails.Create(x.HealthInformation, x.IsCastrated, x.IsVaccinated));

        RuleFor(c => c.Address)
            .MustBeValueObject(x => Address.Create(x.Country, x.City, x.Street, x.Postalcode));

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

        RuleFor(c => c.BreedAndSpeciesId)
            .MustBeValueObject(x => BreedAndSpeciesId.Create(x.SpeciesId, x.BreedId));
    }
}