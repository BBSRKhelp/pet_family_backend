using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Volunteer.Contracts;

namespace PetFamily.Species.Application.Feature.Commands.Species.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IValidator<DeleteSpeciesCommand> validator,
        IVolunteerContract volunteerContract,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _volunteerContract = volunteerContract;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteSpeciesCommand command, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting species");
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Deleting species failed");
            return validationResult.ToErrorList();
        }
        
        var isExist = await _volunteerContract.PetExistsBySpeciesIdAsync(command.Id, cancellationToken);
        if (isExist)
        {
            _logger.LogError("Deleting species failed. There is at least one pet with this species");
            return (ErrorList)Errors.General.IsAssociated("species", "pet");
        }
        
        var speciesResult = await _speciesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogError("Deleting species failed");
            return (ErrorList)speciesResult.Error;
        }
        
        _speciesRepository.Delete(speciesResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("The species with id = {SpeciesId} has been deleted", command.Id);

        return speciesResult.Value.Id.Value;
    }
}