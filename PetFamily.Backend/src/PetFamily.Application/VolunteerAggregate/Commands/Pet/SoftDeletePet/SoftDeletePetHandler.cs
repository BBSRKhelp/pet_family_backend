using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Core.Extensions;
using PetFamily.Core.Interfaces.Abstractions;
using PetFamily.Core.Interfaces.Database;
using PetFamily.Core.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SoftDeletePet;

public class SoftDeletePetHandler : ICommandHandler<Guid, SoftDeletePetCommand>
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
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
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
        
        return petResult.Value.Id.Value;
    }
}