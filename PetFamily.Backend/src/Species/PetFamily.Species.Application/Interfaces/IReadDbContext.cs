using PetFamily.Species.Contracts.DTOs;

namespace PetFamily.Species.Application.Interfaces;

public interface IReadDbContext 
{
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedDto> Breeds { get; }
}