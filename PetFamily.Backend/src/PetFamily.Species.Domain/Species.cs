using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects.Ids;

namespace PetFamily.Species.Domain;

public class Species : CSharpFunctionalExtensions.Entity<SpeciesId>
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

    public Result<Breed, Error> GetBreedById(BreedId id)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == id);
        
        if (breed is null)
            return Errors.General.NotFound(nameof(breed));
        
        return breed;
    }

    public void DeleteBreed(Breed breed)
    {
        _breeds.Remove(breed);
    }
}