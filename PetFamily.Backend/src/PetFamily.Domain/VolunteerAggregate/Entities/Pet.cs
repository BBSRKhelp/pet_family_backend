using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.Enums;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

namespace PetFamily.Domain.VolunteerAggregate.Entities;

public class Pet : CSharpFunctionalExtensions.Entity<PetId>
{
    private bool _isDeleted;
    private readonly List<PetPhoto> _petPhotos = [];

    //ef core
    private Pet() : base(PetId.NewId())
    {
    }

    public Pet(
        Name name,
        Description description,
        AppearanceDetails appearanceDetails,
        HealthDetails healthDetails,
        Address address,
        PhoneNumber phoneNumber,
        DateOnly? birthday,
        StatusForHelp status,
        IReadOnlyList<Requisite> requisites,
        BreedAndSpeciesId breedAndSpeciesId)
        : base(PetId.NewId())
    {
        Name = name;
        Description = description;
        AppearanceDetails = appearanceDetails;
        HealthDetails = healthDetails;
        Address = address;
        PhoneNumber = phoneNumber;
        Birthday = birthday;
        Status = status;
        Requisites = requisites;
        BreedAndSpeciesId = breedAndSpeciesId;
    }

    public Name Name { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public AppearanceDetails AppearanceDetails { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public DateOnly? Birthday { get; private set; }
    public StatusForHelp Status { get; private set; }
    public HealthDetails HealthDetails { get; private set; } = null!;
    public IReadOnlyList<Requisite> Requisites { get; private set; } = [];
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos.AsReadOnly();
    public Position Position { get; private set; } = null!;
    public BreedAndSpeciesId BreedAndSpeciesId { get; private set; } = null!;
    public static DateTime CreatedAt => DateTime.Now;

    public void AddPhotos(IEnumerable<PetPhoto> photos) =>
        _petPhotos.AddRange(photos);

    public void SetSerialNumber(Position position) =>
        Position = position;

    public void Move(Position newPosition) =>
        Position = newPosition;

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void IsDeactivate() => _isDeleted = true;
    public void IsActivate() => _isDeleted = false;
}