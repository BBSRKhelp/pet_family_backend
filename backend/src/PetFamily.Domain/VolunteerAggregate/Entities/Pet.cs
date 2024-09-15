using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.Entities;

public class Pet : Shared.Models.Entity<PetId>
{
    //ef core
    private Pet() : base(PetId.NewId())
    {
    }

    private Pet(
        string nickname,
        string description,
        AppearanceDetails appearanceDetails,
        HealthDetails healthDetails,
        Address address,
        string phoneNumber,
        DateOnly birthday,
        StatusForHelp status,
        PetDetails details) 
        : base (PetId.NewId())
    {
        Nickname = nickname;
        Description = description;
        AppearanceDetails = appearanceDetails;
        HealthDetails = healthDetails;
        Address = address;
        PhoneNumber = phoneNumber;
        Birthday = birthday;
        Status = status;
        Details = details;
    }

    public string? Nickname { get; private set; }
    public string? Description { get; private set; }
    public AppearanceDetails AppearanceDetails { get; private set; }
    public HealthDetails HealthDetails { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateOnly? Birthday { get; private set; }
    public StatusForHelp Status { get; private set; }
    public PetDetails Details { get; private set; }
    public DateTime CreatedAt => DateTime.Now;
    
    //Result Pattern
    public static Result<Pet> Create(
        string nickname,
        string description,
        AppearanceDetails appearanceDetails,
        HealthDetails healthDetails,
        Address address,
        string phoneNumber,
        DateOnly birthday,
        StatusForHelp status,
        PetDetails details)
    {
        if (nickname.Length > 50)
            return Result.Failure<Pet>($"'{nameof(Nickname)}' must be less than 50 characters.");
        
        if (description.Length > 2000)
            return Result.Failure<Pet>($"'{nameof(Description)}' must be less than 2000 characters.");
        
        if (appearanceDetails.SpeciesId == SpeciesId.Empty())
            return Result.Failure<Pet>($"'{nameof(SpeciesId)}' cannot be null or empty.");
        
        if (appearanceDetails.BreedId == BreedId.Empty())
            return Result.Failure<Pet>($"'{nameof(BreedId)}' cannot be null or empty.");

        if (appearanceDetails.Weight <= 0 || appearanceDetails.Weight > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<Pet>("Weight must be between 0 and 1000");
        
        if (appearanceDetails.Height <= 0 || appearanceDetails.Height > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<Pet>("Height must be between 0 and 1000");
        
        if (IsNullOrEmpty(healthDetails.HealthInformation))
            return Result.Failure<Pet>("Health information cannot be null or empty.");
        
        if (!phoneNumber.Contains(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$"))
            return Result.Failure<Pet>("Invalid phone number.");
        
        var pet = new Pet(
            nickname,
            description, 
            appearanceDetails,
            healthDetails,
            address,
            phoneNumber, 
            birthday, 
            status, 
            details);
        
        return Result.Success(pet);
    }
}

