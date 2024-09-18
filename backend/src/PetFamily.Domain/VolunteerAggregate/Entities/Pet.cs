using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
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
        PhoneNumber phoneNumber,
        DateOnly birthday,
        StatusForHelp status,
        PetDetails details,
        BreedAndSpeciesId breedAndSpeciesId) 
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
        BreedAndSpeciesId = breedAndSpeciesId;
    }

    public string? Nickname { get; private set; }
    public string? Description { get; private set; }
    public AppearanceDetails AppearanceDetails { get; private set; } = null!;
    public HealthDetails HealthDetails { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public DateOnly? Birthday { get; private set; }
    public StatusForHelp Status { get; private set; }
    public PetDetails Details { get; private set; } = null!;
    public BreedAndSpeciesId BreedAndSpeciesId { get; private set; } = null!;
    public DateTime CreatedAt => DateTime.Now;
    
    //Result Pattern
    public static Result<Pet> Create(
        string nickname,
        string description,
        AppearanceDetails appearanceDetails,
        HealthDetails healthDetails,
        Address address,
        PhoneNumber phoneNumber,
        DateOnly birthday,
        StatusForHelp status,
        PetDetails details,
        BreedAndSpeciesId breedAndSpeciesId)
    {
        if (nickname.Length > 50)
            return Result.Failure<Pet>($"'{nameof(Nickname)}' must be less than 50 characters.");
        
        if (description.Length > 2000)
            return Result.Failure<Pet>($"'{nameof(Description)}' must be less than 2000 characters.");
        
        var pet = new Pet(
            nickname,
            description, 
            appearanceDetails,
            healthDetails,
            address,
            phoneNumber, 
            birthday,  
            status, 
            details,
            breedAndSpeciesId);
        
        return Result.Success(pet);
    }
}

