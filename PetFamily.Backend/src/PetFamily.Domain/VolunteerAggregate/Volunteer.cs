using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.Entities;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Domain.VolunteerAggregate;

public class Volunteer : Shared.Models.Entity<VolunteerId>
{
    private bool _isDeleted = false;
    
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
        SocialNetworksShell? socialNetwork,
        RequisitesShell? requisites)
        : base(VolunteerId.NewId())
    {
        FullName = fullName;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        SocialNetworks = socialNetwork;
        Requisites = requisites;
    }

    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Description? Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public SocialNetworksShell? SocialNetworks { get; private set; }
    public RequisitesShell? Requisites { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

    public int PetsFoundHome() => _pets.Count(p => p.Status == StatusForHelp.FoundHome);
    public int PetsLookingForHome() => _pets.Count(p => p.Status == StatusForHelp.LookingForHome);
    public int PetsNeedHelp() => _pets.Count(p => p.Status == StatusForHelp.NeedsHelp);

    public void UpdateMainInfo(
        FullName fullName,
        Email email,
        Description? description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
    }

    public void UpdateRequisite(RequisitesShell requisite)
    {
        Requisites = requisite;
    }

    public void UpdateSocialNetwork(SocialNetworksShell socialNetworkses)
    {
        SocialNetworks = socialNetworkses;
    }

    public void IsActivate()
    {
        _isDeleted = false;

        foreach (var pet in _pets)
        {
            pet.IsActivate();
        }
    }
    
    public void IsDeactivate()
    {
        _isDeleted = true;

        foreach (var pet in _pets)
            pet.IsDeactivate();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        //validation + logic
        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(PetId id)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == id);
        if (pet is null)
            return Errors.General.NotFound(nameof(pet));

        return pet;
    }
}