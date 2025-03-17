namespace PetFamily.Species.Domain.ValueObjects.Ids;

public class SpeciesId : ValueObject
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
    
    public static SpeciesId NewId() => new(Guid.NewGuid());
    public static SpeciesId Empty() => new(Guid.Empty);
    public static SpeciesId Create(Guid id) => new(id);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Guid(SpeciesId id) => id.Value;
    public static implicit operator SpeciesId(Guid id) => Create(id);
}