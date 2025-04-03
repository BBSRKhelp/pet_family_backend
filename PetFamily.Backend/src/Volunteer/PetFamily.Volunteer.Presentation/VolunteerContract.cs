using Microsoft.EntityFrameworkCore;
using PetFamily.Volunteer.Application.Interfaces;
using PetFamily.Volunteer.Contracts;

namespace PetFamily.Volunteer.Presentation;

public class VolunteerContract(IReadDbContext readDbContext) : IVolunteerContract
{
    public async Task<Boolean> PetExistsByBreedIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await readDbContext.Pets.AnyAsync(p => p.BreedId == id, cancellationToken);
    }

    public async Task<Boolean> PetExistsBySpeciesIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await readDbContext.Pets.AnyAsync(p => p.SpeciesId == id, cancellationToken);
    }
}