using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetVolunteerByIdQuery> _validator;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;

    public GetVolunteerByIdHandler(
        IReadDbContext readDbContext,
        IValidator<GetVolunteerByIdQuery> validator,
        ILogger<GetVolunteerByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> HandleAsync(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting volunteer by id");

        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Failed to get volunteer by id");
            return validationResult.ToErrorList();
        }

        try
        {
            var volunteerQuery = await _readDbContext
                .Volunteers
                .AsNoTracking()
                .Where(v => v.IsDeleted == false)
                .FirstOrDefaultAsync(v => v.Id == query.VolunteerId, cancellationToken);

            if (volunteerQuery is null)
            {
                _logger.LogInformation("Volunteer with id = '{VolunteerId}' does not found'", query.VolunteerId);
                return (ErrorList)Errors.General.NotFound(nameof(volunteerQuery));
            }

            return volunteerQuery;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get volunteer by id");
            return (ErrorList)Errors.Database.IsFailure();
        }
    }
}