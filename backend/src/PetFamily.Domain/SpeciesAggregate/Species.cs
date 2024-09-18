using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using static System.String;

namespace PetFamily.Domain.SpeciesAggregate;

public class Species : Shared.Models.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    //ef core
    private Species() : base(SpeciesId.NewId())
    {
    }

    private Species(
        string name,
        IEnumerable<Breed> breeds)
        : base(SpeciesId.NewId())
    {
        Name= name;
        _breeds.AddRange(breeds);
    }
    
    public string Name { get; private set; } = null!;
    public IReadOnlyList<Breed> Breeds => _breeds.AsReadOnly();

    public static Result<Species> Create(string name, IEnumerable<Breed>? breeds )
    {
        if (IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Species name cannot be null or empty.");
        
        var species = new Species(name, breeds ?? []);
        
        return Result.Success(species);
    }
}