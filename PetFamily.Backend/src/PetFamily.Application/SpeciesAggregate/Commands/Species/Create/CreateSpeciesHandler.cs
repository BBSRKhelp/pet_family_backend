using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.SpeciesAggregate.Commands.Species.Create;

public class CreateSpeciesHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IValidator<CreateSpeciesCommand> validator,
        ILogger<CreateSpeciesHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating Species");

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var name = Name.Create(command.Name).Value;
        
        var speciesName = await _speciesRepository.GetByNameAsync(name, cancellationToken);
        if (speciesName.IsSuccess)
        {
            _logger.LogWarning("Species with name = {species} already exists", speciesName.Value.Name.Value);
            return (ErrorList)Errors.General.IsExisted("species");
        }

        var species = new Domain.SpeciesAggregate.Species(name);

        var result = await _speciesRepository.AddAsync(species, cancellationToken);

        _logger.LogInformation("The species was created with the ID: {speciesId}", result);

        return result;
    }
}