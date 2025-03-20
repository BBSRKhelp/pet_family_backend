using PetFamily.Species.Application.DTOs.Read;

namespace PetFamily.Species.Application.Interfaces;

public interface IReadDbContext
{
    // IQueryable<VolunteerDto> Volunteers { get; }
    // IQueryable<PetDto> Pets { get; }
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<BreedDto> Breeds { get; }
}