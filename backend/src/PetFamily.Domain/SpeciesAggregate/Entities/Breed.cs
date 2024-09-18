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
    
    private Breed(string name) : base(BreedId.NewId())
    {
        Name = name;
    }

    public string Name { get; } = null!;

    public static Result<Breed> Create(string name)
    {
        if (IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>($"\"name\" cannot be null or empty.");
        
        var breed = new Breed(name);

        return Result.Success(breed);
    }
}