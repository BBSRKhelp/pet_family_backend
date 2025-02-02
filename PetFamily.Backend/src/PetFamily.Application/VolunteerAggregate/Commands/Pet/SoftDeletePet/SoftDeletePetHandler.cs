using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SoftDeletePet;

public class SoftDeletePetHandler : ICommandHandler<Boolean, SoftDeletePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SoftDeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;

    public SoftDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<SoftDeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<Boolean, ErrorList>> HandleAsync(
        SoftDeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting soft-deletion pet with id = {PetId}", command.PetId);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Failed soft-deletion pet");
            return validationResult.ToErrorList();
        }
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Failed soft-deletion pet");
            return (ErrorList)volunteerResult.Error;
        }
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
        {
            _logger.LogWarning("Failed soft-deletion pet");
            return (ErrorList)petResult.Error;
        }
        
        petResult.Value.IsDeactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully soft-deletion pet with id = {PetId}", command.PetId);
        
        return true;
    }
}