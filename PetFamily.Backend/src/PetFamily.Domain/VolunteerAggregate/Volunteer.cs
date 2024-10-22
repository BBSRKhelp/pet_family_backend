using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.Entities;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate;

public class Volunteer : Shared.Models.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    //ef core
    private Volunteer() : base(VolunteerId.NewId())
    {
    }

    public Volunteer(
        FullName fullName,
        Email email,
        Description? description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber,
        SocialNetworkShell? socialNetwork,
        RequisitesShell? requisite)
        : base(VolunteerId.NewId())
    {
        FullName = fullName;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        SocialNetwork = socialNetwork;
        Requisite = requisite;
    }

    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Description? Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public SocialNetworkShell? SocialNetwork { get; private set; }
    public RequisitesShell? Requisite { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

    public int PetsFoundHome() => _pets.Count(p => p.Status == StatusForHelp.FoundHome);
    public int PetsLookingForHome() => _pets.Count(p => p.Status == StatusForHelp.LookingForHome);
    public int PetsNeedHelp() => _pets.Count(p => p.Status == StatusForHelp.NeedsHelp);
}