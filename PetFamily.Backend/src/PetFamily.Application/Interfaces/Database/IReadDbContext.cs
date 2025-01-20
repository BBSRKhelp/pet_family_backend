using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Interfaces.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}