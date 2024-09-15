using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using static System.String;

namespace PetFamily.Domain.SpeciesAggregate.Entities;

public class Breed : Shared.Models.Entity<BreedId>
{
    //ef core
    private Breed() : base(BreedId.NewId())
    {
    }
    
    private Breed(string breedPet) : base(BreedId.NewId())
    {
        BreedPet = breedPet;
    }

    public string BreedPet { get; }

    public static Result<Breed> Create(string breedPet)
    {
        if (IsNullOrWhiteSpace(breedPet))
            return Result.Failure<Breed>($"\"name\" cannot be null or empty.");
        
        var breed = new Breed(breedPet);

        return Result.Success(breed);
    }
}