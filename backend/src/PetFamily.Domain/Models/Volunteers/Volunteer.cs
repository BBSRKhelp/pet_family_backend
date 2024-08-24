using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Pets;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer
{
    //ef core
    public Volunteer()
    {
    }

    public Volunteer(string fullname, string email, string description, byte workExperience, string phoneNumber, Requisites requisites)
    {
        Fullname = fullname;
        Email = email;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
    }

    public Guid Id { get; set; }

    public string Fullname { get; set; }

    public string Email { get; set; }

    public string Description { get; set; }

    public byte WorkExperience { get; set; }

    public string PhoneNumber { get; set; }

    public Requisites Requisites { get; set; }

    private List<Pet> Pets { get; set; } = [];

    public int PetsFoundHome() => Pets.Count(p => p.Status == StatusForHelp.FoundHome);

    public int PetsLookingForHome() => Pets.Count(p => p.Status == StatusForHelp.LookingForHome);
    
    public int PetsNeedHelp() => Pets.Count(p => p.Status == StatusForHelp.NeedsHelp);

    public Result<Volunteer> Create(string fullname, string email, string description, byte workExperience,
        string phoneNumber, Requisites requisites)
    {
        if (fullname.Length > 50)
            return Result.Failure<Volunteer>("Full name must be less than 50 characters.");
        if (!email.Contains("@"))
            return Result.Failure<Volunteer>("Invalid email address.");
        //...
        //...
        
        var volunteer = new Volunteer(fullname, email, description, workExperience, phoneNumber, requisites);
        
        return Result.Success(volunteer);
    }
}