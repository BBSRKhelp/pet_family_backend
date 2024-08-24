using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Pets;

public class Pet
{
    //ef core
    private Pet()
    {
    }

    private Pet(string nickname, string description, string breed,string healthInformation, Address address,
        double weight, double height, string phoneNumber, bool infertility,DateOnly birthday, bool vaccinated,
        StatusForHelp status, List<Requisites> requisites, Colour colour, Species species)
    {
        Nickname = nickname;
        Status = status;
        Description = description;
        Breed = breed;
        Colour = colour;
        HealthInformation = healthInformation;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        Infertility = infertility;
        Birthday = birthday;
        Vaccinated = vaccinated;
        Status = status;
        Requisites = requisites;
    }
    
    public Guid Id { get; private set; }

    public string? Nickname { get; private set; }

    public Species Species { get; private set; }

    public string? Description { get; private set; }

    public string Breed { get; private set; }

    public Colour Colour { get; private set; }

    public string HealthInformation { get; private set; }

    public Address Address { get; private set; } = null!;

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public string PhoneNumber { get; private set; } = null!;

    public bool Infertility { get; private set; }

    public DateOnly? Birthday { get; private set; }

    public bool Vaccinated { get; private set; }

    public StatusForHelp Status { get; private set; }

    public List<Requisites> Requisites { get; private set; } = null!;

    public List<PetPhoto> PetPhotos { get; set; } = null!;

    public DateTime CreatedAt => DateTime.Now;
    
    //Result Pattern
    public static Result<Pet> Create(string nickname, string description, string breed,string healthInformation,
        Address address, double weight, double height, string phoneNumber, bool infertility,DateOnly birthday,
        bool vaccinated, StatusForHelp status, List<Requisites> requisites, Colour colour = default, Species species = default)
    {
        if (nickname.Length > 50)
            return Result.Failure<Pet>($"'{nameof(Nickname)}' must be less than 50 characters.");
        if(description.Length > 250)
            return Result.Failure<Pet>($"'{nameof(Description)}' must be less than 250 characters.");
        //...
        //...
        
        var pet = new Pet(nickname, description, breed, healthInformation, address, weight, height,
            phoneNumber, infertility, birthday, vaccinated, status, requisites, colour, species);
        
        return Result.Success(pet);
    }
}