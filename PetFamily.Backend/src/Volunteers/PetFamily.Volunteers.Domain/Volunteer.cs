using CSharpFunctionalExtensions;
using PetFamily.Core.Enums;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.Ids;

namespace PetFamily.Volunteers.Domain;

public class Volunteer() : Entity<VolunteerId>(VolunteerId.NewId()), ISoftDeletable
{
    private bool _isDeleted;
    private readonly List<Pet> _pets = [];

    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

    public int PetsFoundHome() => _pets.Count(p => p.Status == Status.FoundHome);
    public int PetsLookingForHome() => _pets.Count(p => p.Status == Status.LookingForHome);
    public int PetsUndergoingTreatment() => _pets.Count(p => p.Status == Status.UndergoingTreatment);

    public Result<Pet, Error> GetPetById(PetId id)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == id);
        if (pet is null)
            return Errors.General.NotFound(nameof(pet));
        
        return pet;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;

        pet.SetPosition(positionResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
            return UnitResult.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.Move(newPosition);

        return UnitResult.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position >= newPosition && p.Position < currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                    return result.Error;
            }
        }

        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets
                .Where(p => p.Position <= newPosition && p.Position > currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                    return result.Error;
            }
        }

        return Result.Success<Error>();
    }

    public void DeletePet(Pet pet) => _pets.Remove(pet);
    
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
}