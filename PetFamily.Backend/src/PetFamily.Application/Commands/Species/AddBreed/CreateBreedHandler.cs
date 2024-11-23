using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.SpeciesAggregate.Entities;

namespace PetFamily.Application.Commands.Species.AddBreed;

public class CreateBreedHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBreedHandler> _logger;

    public CreateBreedHandler(
        ISpeciesRepository speciesRepository,
        IValidator<CreateBreedCommand> validator,
        IUnitOfWork unitOfWork, 
        ILogger<CreateBreedHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateBreedCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding Breed");
        
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var speciesResult = await _speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogWarning("Failed to add breed");
            return (ErrorList)speciesResult.Error;
        }
        
        var name = Name.Create(command.Name).Value;
        
        var breed = new Breed(name);
        
        var result = speciesResult.Value.AddBreed(breed);
        if (result.IsFailure)
        {
            if (result.Error.Code == "record.is.existed")
                _logger.LogWarning("Breed with name = {breed} already exists", breed.Name.Value);
            return (ErrorList)result.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
        
        _logger.LogInformation("Breed with Id = {Id} added", breed.Id.Value);
        
        return breed.Id.Value;
    }
}