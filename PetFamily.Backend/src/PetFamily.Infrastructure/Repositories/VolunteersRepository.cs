using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

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

        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public Guid SaveChanges(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with Id = {volunteerId} was not found", volunteerId.Value);

            return Errors.General.NotFound(nameof(volunteerId));
        }

        _logger.LogInformation("A volunteer with Id = {volunteerId} has been found", volunteerId.Value);

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneAsync(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with Phone = {phoneNumber} was not found", phoneNumber.Value);

            return Errors.General.NotFound(nameof(phoneNumber));
        }

        _logger.LogInformation("A volunteer with Phone = {phoneNumber} has been found", phoneNumber.Value);

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with Email = {email} was not found", email.Value);

            return Errors.General.NotFound(nameof(email));
        }

        _logger.LogInformation("A volunteer with Email = {email} has been found", email.Value);

        return volunteer;
    }
}