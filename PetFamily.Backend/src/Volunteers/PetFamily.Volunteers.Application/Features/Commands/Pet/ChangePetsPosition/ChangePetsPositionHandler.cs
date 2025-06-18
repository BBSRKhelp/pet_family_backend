using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.ChangePetsPosition;

public class ChangePetsPositionHandler : ICommandHandler<ChangePetsPositionCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<ChangePetsPositionCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePetsPositionHandler> _logger;

    public ChangePetsPositionHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<ChangePetsPositionCommand> validator,
        [FromKeyedServices(UnitOfWorkContext.Volunteers)]IUnitOfWork unitOfWork,
        ILogger<ChangePetsPositionHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(
        ChangePetsPositionCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Change pets position started");

        var validation = await _validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            _logger.LogWarning("Change pets position failed");
            return validation.ToErrorList();
        }
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Change pets position failed");
            return (ErrorList)volunteerResult.Error;
        }

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Change pets position failed");
            return (ErrorList)petResult.Error;
        }

        var newPosition = Position.Create(command.NewPosition);
        if (newPosition.IsFailure)
        {
            _logger.LogWarning("Change pets position failed");
            return (ErrorList)newPosition.Error;
        }

        var result = volunteerResult.Value.MovePet(petResult.Value, newPosition.Value);
        if (result.IsFailure)
        { 
            _logger.LogWarning("Change pets position failed");
            return (ErrorList)result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Change pets position success");
        
        return Result.Success<ErrorList>();
    }
}