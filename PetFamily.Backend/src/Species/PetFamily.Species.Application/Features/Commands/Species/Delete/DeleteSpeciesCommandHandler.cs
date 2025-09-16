using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Features.Commands.Species.Delete;

public class DeleteSpeciesCommandHandler(
    ISpeciesRepository speciesRepository,
    IVolunteerContract volunteerContract,
    [FromKeyedServices(UnitOfWorkContext.Species)]
    IUnitOfWork unitOfWork,
    IValidator<DeleteSpeciesCommand> validator,
    ILogger<DeleteSpeciesCommandHandler> logger)
    : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteSpeciesCommand command, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting species");
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogError("Deleting species failed");
            return validationResult.ToErrorList();
        }
        
        var isExist = await volunteerContract.PetExistsBySpeciesIdAsync(command.Id, cancellationToken);
        if (isExist)
        {
            logger.LogError("Deleting species failed. There is at least one pet with this species");
            return (ErrorList)Errors.General.IsAssociated("species", "pet");
        }
        
        var speciesResult = await speciesRepository.GetByIdAsync(command.Id, cancellationToken);
        if (speciesResult.IsFailure)
        {
            logger.LogError("Deleting species failed");
            return (ErrorList)speciesResult.Error;
        }
        
        speciesRepository.Delete(speciesResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("The species with id = {SpeciesId} has been deleted", command.Id);

        return speciesResult.Value.Id.Value;
    }
}