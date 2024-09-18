using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.Models.Pets;

public class Pet : Shared.Entity<PetId>
{
    //ef core
    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId petId,
        string nickname,
        string description,
        Breed breed,
        string healthInformation,
        Address address,
        double weight,
        double height,
        string phoneNumber,
        bool isInfertility,
        DateOnly birthday,
        bool isVaccinated,
        StatusForHelp status,
        List<Requisite> requisites,
        Colour colour,
        Species species) 
        : base (petId)
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
        IsInfertility = isInfertility;
        Birthday = birthday;
        IsVaccinated = isVaccinated;
        Status = status;
        Requisites = requisites;
    }
    
    public string? Nickname { get; private set; }
    public Species Species { get; private set; }
    public string? Description { get; private set; }
    public Breed Breed { get; private set; }
    public Colour Colour { get; private set; } = default!;
    public string HealthInformation { get; private set; }
    public Address Address { get; private set; } = null!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public string PhoneNumber { get; private set; } = null!;
    public bool IsInfertility { get; private set; }
    public DateOnly? Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public StatusForHelp Status { get; private set; } = default!;
    public List<Requisite> Requisites { get; private set; } = null!;
    public DateTime CreatedAt => DateTime.Now;
    public List<PetPhoto> PetPhotos { get; set; } = null!;
    
    //Result Pattern
    public static Result<Pet> Create(
        PetId id,
        string nickname,
        string description,
        Breed breed,
        string healthInformation,
        Address address,
        double weight,
        double height, 
        string phoneNumber,
        bool isInfertility,
        DateOnly birthday,
        bool isVaccinated, 
        StatusForHelp status, 
        List<Requisite> requisites, 
        Colour colour,
        Species species)
    {
        if (nickname.Length > 50)
            return Result.Failure<Pet>($"'{nameof(Nickname)}' must be less than 50 characters.");
        
        if (description.Length > 2000)
            return Result.Failure<Pet>($"'{nameof(Description)}' must be less than 2000 characters.");
        //TODO Validation
        
        if (IsNullOrEmpty(healthInformation))
            return Result.Failure<Pet>($"'{nameof(HealthInformation)}' cannot be null or empty.");
        
        if (weight <= 0)
            return Result.Failure<Pet>($"'{nameof(Weight)}' must be greater than 0.");
        
        if (height <= 0)
            return Result.Failure<Pet>($"'{nameof(Height)}' must be greater than 0.");
        
        if (!phoneNumber.Contains(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$"))
            return Result.Failure<Pet>("Invalid phone number.");
 
        
                
        
        var pet = new Pet(
            id,
            nickname,
            description, 
            breed, 
            healthInformation, 
            address,
            weight, 
            height,
            phoneNumber, 
            isInfertility, 
            birthday, 
            isVaccinated,
            status, 
            requisites, 
            colour, 
            species);
        
        return Result.Success(pet);
    }
}

