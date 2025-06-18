using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Domain.ValueObjects.Ids;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Infrastructure.Database;

namespace PetFamily.Volunteers.Infrastructure;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _dbContext;
    private readonly ILogger<VolunteersRepository> _logger;

    public VolunteersRepository(WriteDbContext dbContext, ILogger<VolunteersRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer)
    {
        _dbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetByIdAsync(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with id = {volunteerId} was not found", volunteerId.Value);

            return Errors.General.NotFound(nameof(volunteerId));
        }

        _logger.LogInformation("A volunteer with id = {volunteerId} has been found", volunteerId.Value);

        return volunteer;
    }
}