using PetFamily.Volunteer.Contracts.DTOs;

namespace PetFamily.Volunteer.Application.Interfaces;

public interface IReadDbContext 
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}