using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.Models.Pets;

public class Species
{
    private Species()
    {
    }

    private Species(string nameOfSpecies, IReadOnlyList<Breed> breed)
    {
        NameOfSpecies = nameOfSpecies;
        Breeds = breed;
    }
    
    public Guid Id { get; private set; }
    public string NameOfSpecies { get; private set; }
    public IReadOnlyList<Breed> Breeds { get; } = null!;

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