using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.Commands.Volunteer.Delete;

public class VolunteerDeleteHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerCreateHandler> _logger;

    public VolunteerDeleteHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerCreateHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        volunteerResult.Value.IsDeactivate();

        var result = await _volunteersRepository.SaveChangesAsync(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("The volunteer with id = {VolunteerId} has been deleted", result);

        return result;
    }
}