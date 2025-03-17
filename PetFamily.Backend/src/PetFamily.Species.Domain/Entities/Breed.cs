using PetFamily.Species.Domain.ValueObjects.Ids;

namespace PetFamily.Species.Domain.Entities;

public class Breed : Entity<BreedId>
{
    //ef core
    private Breed() : base(BreedId.NewId())
    {
    }

    public Breed(Name name) :
        base(BreedId.NewId())
    {
        Name = name;
    }

    public Name Name { get; } = null!;
}