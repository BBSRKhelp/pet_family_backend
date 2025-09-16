using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Features.Commands.Breed.DeleteBreed;

public class DeleteBreedCommandHandler(
    ISpeciesRepository speciesRepository,
    IValidator<DeleteBreedCommand> validator,
    IVolunteerContract volunteerContract,
    [FromKeyedServices(UnitOfWorkContext.Species)]
    IUnitOfWork unitOfWork,
    ILogger<DeleteBreedCommandHandler> logger)
    : ICommandHandler<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting breed");

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogError("Deleting breed failed");
            return validationResult.ToErrorList();
        }

        var petExists = await volunteerContract.PetExistsByBreedIdAsync(command.BreedId, cancellationToken);
        if (petExists)
        {
            logger.LogError("Deleting breed failed. There is at least one pet with this breed");
            return (ErrorList)Errors.General.IsAssociated("breed", "pet");
        }

        var speciesResult = await speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            logger.LogError("Deleting breed failed");
            return (ErrorList)speciesResult.Error;
        }

        var breedResult = speciesResult.Value.GetBreedById(command.BreedId);
        if (breedResult.IsFailure)
        {
            logger.LogError("Deleting breed failed");
            return (ErrorList)breedResult.Error;
        }

        speciesResult.Value.DeleteBreed(breedResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("The breed with id = {BreedId} has been deleted", command.BreedId);

        return breedResult.Value.Id.Value;
    }
}