using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteer.Application.Interfaces;
using PetFamily.Volunteer.Domain.ValueObjects;
using PetFamily.Volunteer.Domain.ValueObjects.Ids;
using PetFamily.Volunteer.Infrastructure.Database;

namespace PetFamily.Volunteer.Infrastructure;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _dbContext;
    private readonly ILogger<VolunteersRepository> _logger;

    public VolunteersRepository(WriteDbContext dbContext, ILogger<VolunteersRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Domain.Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id.Value;
    }

    public Guid Delete(Domain.Volunteer volunteer)
    {
        _dbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Domain.Volunteer, Error>> GetByIdAsync(
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

    public async Task<Result<Domain.Volunteer, Error>> GetByPhoneAsync(PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with phone = {phoneNumber} was not found", phoneNumber.Value);

            return Errors.General.NotFound(nameof(phoneNumber));
        }

        _logger.LogInformation("A volunteer with phone = {phoneNumber} has been found", phoneNumber.Value);

        return volunteer;
    }

    public async Task<Result<Domain.Volunteer, Error>> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        var volunteer = await _dbContext
            .Volunteers
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);

        if (volunteer is null)
        {
            _logger.LogInformation("A volunteer with email = {email} was not found", email.Value);

            return Errors.General.NotFound(nameof(email));
        }

        _logger.LogInformation("A volunteer with email = {email} has been found", email.Value);

        return volunteer;
    }
}