using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Domain.SpeciesAggregate;

public class Species : Shared.Models.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    //ef core
    private Species() : base(SpeciesId.NewId())
    {
    }

    public Species(Name name)
        : base(SpeciesId.NewId())
    {
        Name = name;
    }

    public Name Name { get; private set; } = null!;
    public IReadOnlyList<Breed> Breeds => _breeds.AsReadOnly();

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (_breeds.Any(b => b.Name == breed.Name))
            return Errors.General.IsExisted(nameof(breed));
        
        _breeds.Add(breed);
        
        return Result.Success<Error>();
    }
}