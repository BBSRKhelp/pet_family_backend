using PetFamily.Core.Models;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Species.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IValidator<DeleteSpeciesCommand> validator,
        IReadDbContext readDbContext,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _readDbContext = readDbContext;
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
        
        var isExist = await _readDbContext.Pets.AnyAsync(p => p.SpeciesId == command.Id, cancellationToken);
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