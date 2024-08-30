using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Pets;
using static System.String;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Shared.Entity<VolunteerId>
{
    //ef core
    private Volunteer(VolunteerId volunteerId) : base(volunteerId)
    {
    }

    private Volunteer(
        VolunteerId volunteerId,
        string fullname, 
        string email, 
        string description,
        byte workExperience, 
        string phoneNumber, 
        List<Requisite> requisites)
        : base(volunteerId)
    {
        Fullname = fullname;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
    }

    public string Fullname { get; private set; }
    public string Email { get; private set; }
    public string Description { get; private set; }
    public byte WorkExperience { get; private set; }
    public string PhoneNumber { get; private set; }
    public List<Requisite> Requisites  { get; private set; }
    private List<Pet> Pets { get; set; } = [];

    public int PetsFoundHome() => Pets.Count(p => p.Status == StatusForHelp.FoundHome);
    public int PetsLookingForHome() => Pets.Count(p => p.Status == StatusForHelp.LookingForHome);
    public int PetsNeedHelp() => Pets.Count(p => p.Status == StatusForHelp.NeedsHelp);

    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        string fullname, 
        string email, 
        string description, 
        byte workExperience,
        string phoneNumber,
        List<Requisite> requisites)
    {
        if (fullname.Length > 50 || IsNullOrWhiteSpace(fullname))
            return Result.Failure<Volunteer>("Full name must be less than 50 characters.");
        
        if (!email.Contains(@"^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$"))
            return Result.Failure<Volunteer>("Invalid email address.");
        
        if (description.Length > 2000)
            return Result.Failure<Volunteer>("Description must be less than 2000 characters.");
        
        if (IsNullOrWhiteSpace(workExperience.ToString()))
            return Result.Failure<Volunteer>("Invalid work experience.");
        
        if (!phoneNumber.Contains(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$"))
            return Result.Failure<Volunteer>("Invalid phone number.");
        
        var volunteer = new Volunteer(
            volunteerId,
            fullname, 
            email, 
            description,
            workExperience,
            phoneNumber, 
            requisites);
        
        return Result.Success(volunteer);
    }
}