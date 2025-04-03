using Microsoft.EntityFrameworkCore;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation;

public class SpeciesContract(IReadDbContext readDbContext) : ISpeciesContract
{
    public async Task<Boolean> SpeciesExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await readDbContext.Species.AnyAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Boolean> BreedExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await readDbContext.Breeds.AnyAsync(b => b.Id == id, cancellationToken);
    }
}