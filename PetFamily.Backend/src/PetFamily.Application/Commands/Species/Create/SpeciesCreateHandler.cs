using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Commands.Species.Create;

public class SpeciesCreateHandler
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<SpeciesCreateHandler> _logger;

    public SpeciesCreateHandler(
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork, // надо ли?
        ILogger<SpeciesCreateHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        SpeciesCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating Species");

        var name = Name.Create(command.Name).Value;
        
        var speciesName = await _speciesRepository.GetByNameAsync(name, cancellationToken);
        if (speciesName.IsSuccess)
        {
            _logger.LogWarning("Species with name = {species} already exists", speciesName.Value.Name.Value);
            return Errors.General.IsExisted("species");
        }

        var species = new Domain.SpeciesAggregate.Species(name);

        var result = await _speciesRepository.AddAsync(species, cancellationToken);

        _logger.LogInformation("The species was created with the ID: {speciesId}", result);

        return result;
    }
}