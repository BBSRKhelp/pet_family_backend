using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public class VolunteerUpdateRequisitesHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateRequisitesHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteerUpdateRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateRequisitesHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Updating the volunteer's requisites");

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Volunteer update failed");
                return volunteerResult.Error;
            }

            var requisites = command.Requisites
                .Select(r => Requisite.Create(r.Title, r.Description).Value);

            volunteerResult.Value.UpdateRequisite(new RequisitesShell(requisites));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

            transaction.Commit();

            return volunteerResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update the volunteer's requisites");

            transaction.Rollback();

            return Error.Failure("update.requisites.volunteer", "Failed to update the volunteer's requisites");
        }
    }
}