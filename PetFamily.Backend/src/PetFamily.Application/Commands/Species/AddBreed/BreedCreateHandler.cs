using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesAggregate.Entities;

namespace PetFamily.Application.Commands.Species.AddBreed;

public class BreedCreateHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BreedCreateHandler> _logger;

    public BreedCreateHandler(
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork, 
        ILogger<BreedCreateHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(BreedCreateCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding Breed");
        
        var speciesResult = await _speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogWarning("Failed to add breed");
            return speciesResult.Error;
        }
        
        var name = Name.Create(command.Name).Value;
        
        var breed = new Breed(name);
        
        var result = speciesResult.Value.AddBreed(breed);
        if (result.IsFailure)
        {
            if (result.Error.Code == "record.is.existed")
                _logger.LogWarning("Breed with name = {breed} already exists", breed.Name.Value);
            return result.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Breed with Id = {Id} added", breed.Id.Value);
        
        return breed.Id.Value;
    }
}