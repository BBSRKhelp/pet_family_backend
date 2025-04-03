namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<Boolean> SpeciesExistsAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Boolean> BreedExistsAsync(Guid id, CancellationToken cancellationToken);
}