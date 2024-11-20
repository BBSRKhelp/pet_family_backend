using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.Commands.Volunteer.Delete;

public class VolunteerDeleteHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerCreateHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteerDeleteHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerCreateHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Deleting Volunteer");

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Volunteer delete failed");
                return volunteerResult.Error;
            }

            volunteerResult.Value.IsDeactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("The volunteer with id = {VolunteerId} has been deleted", command.Id);

            transaction.Commit();
            
            return volunteerResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete Volunteer");
            
            transaction.Rollback();
            
            return Error.Failure("delete.volunteer" , "Failed to delete Volunteer");
        }
    }
}