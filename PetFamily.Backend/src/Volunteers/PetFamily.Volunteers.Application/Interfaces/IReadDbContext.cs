using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Application.Interfaces;

public interface IReadDbContext 
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}