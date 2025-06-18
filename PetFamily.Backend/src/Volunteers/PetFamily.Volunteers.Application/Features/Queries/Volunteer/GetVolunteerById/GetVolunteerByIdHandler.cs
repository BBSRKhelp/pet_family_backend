using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetVolunteerById;

public class GetVolunteerByIdHandler(
    IReadDbContext readDbContext,
    IValidator<GetVolunteerByIdQuery> validator,
    ILogger<GetVolunteerByIdHandler> logger)
    : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    public async Task<Result<VolunteerDto, ErrorList>> HandleAsync(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting volunteer by id");

        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Failed to get volunteer by id");
            return validationResult.ToErrorList();
        }

        var volunteer = await readDbContext
            .Volunteers
            .Where(v => v.IsDeleted == false)
            .FirstOrDefaultAsync(v => v.Id == query.VolunteerId, cancellationToken);

        if (volunteer is not null) return volunteer;

        logger.LogInformation("Volunteer with id = '{VolunteerId}' does not found'", query.VolunteerId);
        return (ErrorList)Errors.General.NotFound("volunteer");
    }
}