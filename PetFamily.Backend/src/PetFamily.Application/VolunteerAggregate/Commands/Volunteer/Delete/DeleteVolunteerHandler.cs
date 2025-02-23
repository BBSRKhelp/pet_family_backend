using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Create;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Delete;

public class DeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeleteVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting Volunteer");

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Volunteer delete failed");
            return (ErrorList)volunteerResult.Error;
        }

        volunteerResult.Value.IsDeactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("The volunteer with id = {VolunteerId} has been deleted", command.Id);

        return command.Id;
    }
}