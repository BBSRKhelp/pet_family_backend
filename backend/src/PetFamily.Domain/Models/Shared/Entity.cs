using PetFamily.Domain.Models.Pets;

namespace PetFamily.Domain.Shared;

public abstract class Entity<TId>
    where TId : notnull
{
    protected Entity(TId value) => Value = value;
    
    public TId Value { get; private set; }
    
    public static TId NewId() => (TId)Convert.ChangeType(Guid.NewGuid(), typeof(TId));

}