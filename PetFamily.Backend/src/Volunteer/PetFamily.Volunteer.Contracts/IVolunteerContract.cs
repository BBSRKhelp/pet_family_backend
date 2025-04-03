namespace PetFamily.Volunteer.Contracts;

public interface IVolunteerContract
{
    Task<Boolean> PetExistsByBreedIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Boolean> PetExistsBySpeciesIdAsync(Guid id, CancellationToken cancellationToken);
}