using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate;
using PetFamily.Domain.VolunteerAggregate.Entities;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.UnitTests.Shared;

public class Models
{
    public static Volunteer CreateVolunteer()
    {
        var fullName = FullName.Create("firstName", "lastName").Value;
        var email = Email.Create("email@email.com").Value;
        var description = Description.Create("description").Value;
        var workExperience = WorkExperience.Create(2).Value;
        var phoneNumber = PhoneNumber.Create("89166988888").Value;
        IReadOnlyList<SocialNetwork> socialNetworkShell = [SocialNetwork.Create("title", "url").Value];
        IReadOnlyList<Requisite> requisiteShell = [Requisite.Create("title", "url").Value];

        return new Volunteer(
            fullName,
            email,
            description,
            workExperience,
            phoneNumber,
            socialNetworkShell,
            requisiteShell);
    }

    public static Pet CreatePet()
    {
        var name = Name.Create("TestPet").Value;
        var description = Description.Create("TestDescription").Value;
        var appearanceDetails = AppearanceDetails.Create(Colour.Orange, 10, 100).Value;
        var healthDetails = HealthDetails.Create("test", true, true).Value;
        var address = Address.Create("test", "test", "test", "test").Value;
        var phoneNumber = PhoneNumber.Create("88888888888").Value;
        var birthday = DateOnly.Parse("2015-01-01");
        var status = Status.LookingForHome;
        IReadOnlyList<Requisite> requisites = [Requisite.Create("TestRequisite", "TestRequisiteUrl").Value];
        var breedAndSpeciesId = BreedAndSpeciesId.Create(SpeciesId.NewId(), Guid.NewGuid()).Value;

        return new Pet(
            name,
            description,
            appearanceDetails,
            healthDetails,
            address,
            phoneNumber,
            birthday,
            status,
            requisites,
            breedAndSpeciesId);
    }
}