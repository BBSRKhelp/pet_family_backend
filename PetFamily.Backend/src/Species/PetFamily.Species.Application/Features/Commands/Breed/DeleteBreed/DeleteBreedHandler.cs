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

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IVolunteerContract _volunteerContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository speciesRepository,
        IValidator<DeleteBreedCommand> validator,
        IVolunteerContract volunteerContract,
        [FromKeyedServices(UnitOfWorkContext.Species)]IUnitOfWork unitOfWork,
        ILogger<DeleteBreedHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _volunteerContract = volunteerContract;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting breed");

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Deleting breed failed");
            return validationResult.ToErrorList();
        }

        var petExists = await _volunteerContract.PetExistsByBreedIdAsync(command.BreedId, cancellationToken);
        if (petExists)
        {
            _logger.LogError("Deleting breed failed. There is at least one pet with this breed");
            return (ErrorList)Errors.General.IsAssociated("breed", "pet");
        }

        var speciesResult = await _speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogError("Deleting breed failed");
            return (ErrorList)speciesResult.Error;
        }
        
        var breedResult = speciesResult.Value.GetBreedById(command.BreedId);
        if (breedResult.IsFailure)
        {
            _logger.LogError("Deleting breed failed");
            return (ErrorList)breedResult.Error;
        }
        
        speciesResult.Value.DeleteBreed(breedResult.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("The breed with id = {BreedId} has been deleted", command.BreedId);
        
        return breedResult.Value.Id.Value;
    }
}