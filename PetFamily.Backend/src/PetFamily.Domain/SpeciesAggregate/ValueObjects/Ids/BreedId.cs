namespace PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

public record BreedId
{
    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static BreedId NewId() => new(Guid.NewGuid());
    public static BreedId Empty() => new(Guid.Empty);
    public static BreedId Create(Guid id) => new(id);
    
    public static implicit operator Guid(BreedId id) => id.Value;
    public static implicit operator BreedId(Guid id) => BreedId.Create(id);
}