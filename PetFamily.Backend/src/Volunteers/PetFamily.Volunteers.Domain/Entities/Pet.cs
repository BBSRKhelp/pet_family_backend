using CSharpFunctionalExtensions;
using PetFamily.Core.Enums;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.Ids;

namespace PetFamily.Volunteers.Domain.Entities;

public class Pet : Entity<PetId>, ISoftDeletable
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
        DateOnly? birthDate,
        Status status,
        BreedAndSpeciesId breedAndSpeciesId)
        : base(PetId.NewId())
    {
        Name = name;
        Description = description;
        AppearanceDetails = appearanceDetails;
        HealthDetails = healthDetails;
        Address = address;
        BirthDate = birthDate;
        Status = status;
        BreedAndSpeciesId = breedAndSpeciesId;
    }

    public Name Name { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public AppearanceDetails AppearanceDetails { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public DateOnly? BirthDate { get; private set; }
    public Status Status { get; private set; }
    public HealthDetails HealthDetails { get; private set; } = null!;
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos.AsReadOnly();
    public Position Position { get; private set; } = null!;
    public BreedAndSpeciesId BreedAndSpeciesId { get; private set; } = null!;
    public static DateTime CreatedAt => DateTime.Now;

    public void AddPhotos(IEnumerable<PetPhoto> photos) =>
        _petPhotos.AddRange(photos);

    public void SetPosition(Position position) =>
        Position = position;

    public void UpdateMainInfo(
        Name name,
        Description description,
        AppearanceDetails appearanceDetails,
        Address address,
        DateOnly? birthday,
        HealthDetails healthDetails,
        BreedAndSpeciesId breedAndSpeciesId)
    {
        Name = name;
        Description = description;
        AppearanceDetails = appearanceDetails;
        Address = address;
        BirthDate = birthday;
        HealthDetails = healthDetails;
        BreedAndSpeciesId = breedAndSpeciesId;
    }

    public void UpdateStatus(Status status)
    {
        Status = status;
    }

    public UnitResult<Error> SetMainPhoto(PhotoPath photoPath)
    {
        var petPhoto = _petPhotos.FirstOrDefault(x => x.PhotoPath == photoPath);
        if (petPhoto is null)
            return Errors.General.NotFound("PetPhoto");

        var currentMainPhoto = _petPhotos.FirstOrDefault(x => x.IsMainPhoto);
        if (currentMainPhoto is not null)
        {
            _petPhotos.Remove(currentMainPhoto);
            _petPhotos.Add(new PetPhoto(currentMainPhoto.PhotoPath));
        }

        _petPhotos.Remove(petPhoto);
        _petPhotos.Add(new PetPhoto(photoPath, true));

        return UnitResult.Success<Error>();
    }

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