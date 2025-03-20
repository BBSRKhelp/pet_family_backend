using CSharpFunctionalExtensions;
using PetFamily.Core.Models;
using PetFamily.Core.ValueObjects;
using PetFamily.Core.ValueObjects.Ids;

namespace PetFamily.Species.Application.Interfaces;

public interface ISpeciesRepository
{
    Task<Guid> AddAsync(Domain.Species species, CancellationToken cancellationToken = default);
    public Guid Delete(Domain.Species species);
    Task<Result<Domain.Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken cancellationToken = default);
    Task<Result<Domain.Species, Error>> GetByNameAsync(Name name, CancellationToken cancellationToken = default);
}