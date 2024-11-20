using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesAggregate;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;

namespace PetFamily.Application.Interfaces.Repositories;

public interface ISpeciesRepository
{
    Task<Guid> AddAsync(Species species, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByIdAsync(SpeciesId speciesId, CancellationToken cancellationToken = default);
    Task<Result<Species, Error>> GetByNameAsync(Name name, CancellationToken cancellationToken = default);
}