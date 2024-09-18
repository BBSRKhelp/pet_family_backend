using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.Entities;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate;

public class Volunteer : Shared.Models.Entity<VolunteerId>
{
    //ef core
    private Volunteer() : base(VolunteerId.NewId())
    {
    }

    private Volunteer(
        Fullname fullname, 
        string email, 
        string description,
        byte workExperience, 
        PhoneNumber phoneNumber,
        VolunteerDetails volunteerDetails,
        List<Pet> pets)
        : base(VolunteerId.NewId())
    {
        Fullname = fullname;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        Details = volunteerDetails;
        Pets = pets;
    }

    public Fullname Fullname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? Description { get; private set; }
    public byte WorkExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public VolunteerDetails Details { get; private set; } = null!;
    public List<Pet>? Pets { get; }

    public int PetsFoundHome() => Pets!.Count(p => p.Status == StatusForHelp.FoundHome);
    public int PetsLookingForHome() => Pets!.Count(p => p.Status == StatusForHelp.LookingForHome);
    public int PetsNeedHelp() => Pets!.Count(p => p.Status == StatusForHelp.NeedsHelp);

    public static Result<Volunteer> Create(
        Fullname fullname, 
        string email, 
        string description, 
        byte workExperience,
        PhoneNumber phoneNumber,
        VolunteerDetails volunteerDetails,
        List<Pet> pets)
    {
        if (!email.Contains(@"^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"))
            return Result.Failure<Volunteer>("Invalid email address.");
        
        if (description.Length > 2000)
            return Result.Failure<Volunteer>("Description must be less than 2000 characters.");
        
        if (IsNullOrWhiteSpace(workExperience.ToString()))
            return Result.Failure<Volunteer>("Invalid work experience.");
        
        var volunteer = new Volunteer(
            fullname, 
            email, 
            description,
            workExperience,
            phoneNumber, 
            volunteerDetails,
            pets);
        
        return Result.Success(volunteer);
    }
}