using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using static System.String;

namespace PetFamily.Domain.SpeciesAggregate;

public class Species : Shared.Models.Entity<SpeciesId>
{
    private Species() : base(SpeciesId.NewId())
    {
    }

    private Species(
        string nameOfSpecies, 
        IReadOnlyList<Breed> breed)
        : base(SpeciesId.NewId())
    {
        NameOfSpecies = nameOfSpecies;
        Breeds = breed;
    }
    
    public string NameOfSpecies { get; private set; }
    public IReadOnlyList<Breed> Breeds { get; }

    public static Result<Species> Create(string name, IReadOnlyList<Breed>? breeds)
    {
        if (IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Species name cannot be null or empty.");
        
        if (breeds == null)
            return Result.Failure<Species>("Breeds cannot be null or empty.");

        var species = new Species(name, breeds);
        
        return Result.Success(species);
    }
}