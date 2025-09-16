using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Application.Interfaces;

namespace PetFamily.Species.Application.Features.Commands.Breed.AddBreed;

public class AddBreedCommandHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddBreedCommandHandler> _logger;

    public AddBreedCommandHandler(
        ISpeciesRepository speciesRepository,
        IValidator<AddBreedCommand> validator,
        [FromKeyedServices(UnitOfWorkContext.Species)]IUnitOfWork unitOfWork, 
        ILogger<AddBreedCommandHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding Breed");
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Failed to add breed");
            return validationResult.ToErrorList();
        }
        
        var speciesResult = await _speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogWarning("Failed to add breed");
            return (ErrorList)speciesResult.Error;
        }
        
        var name = Name.Create(command.Name).Value;
        
        var breed = new Domain.Entities.Breed(name);
        
        var result = speciesResult.Value.AddBreed(breed);
        if (result.IsFailure)
        {
            if (result.Error.Code == "record.is.existed")
                _logger.LogWarning("Breed with name = {breed} already exists", breed.Name.Value);
            return (ErrorList)result.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Breed with Id = {Id} added", breed.Id.Value);
        
        return breed.Id.Value;
    }
}