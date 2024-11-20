namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

public record PetId
{
    private PetId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static PetId NewId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new(id);
    public static implicit operator PetId(Guid id) => PetId.Create(id);
    public static implicit operator Guid(PetId petId) => petId.Value;
}
