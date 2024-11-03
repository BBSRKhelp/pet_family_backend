using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public class VolunteerUpdateRequisitesHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateRequisitesHandler> _logger;

    public VolunteerUpdateRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateRequisitesHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating the volunteer's requisites");
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value);

        volunteerResult.Value.UpdateRequisite(new RequisitesShell(requisites));
        
        var result = await _volunteersRepository.SaveChangesAsync(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", result);
        
        return result;
    }
}