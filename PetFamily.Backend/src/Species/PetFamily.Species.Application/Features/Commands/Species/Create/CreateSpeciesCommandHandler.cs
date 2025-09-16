using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Application.Interfaces;

namespace PetFamily.Species.Application.Features.Commands.Species.Create;

public class CreateSpeciesCommandHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSpeciesCommandHandler> _logger;

    public CreateSpeciesCommandHandler(
        ISpeciesRepository speciesRepository,
        IValidator<CreateSpeciesCommand> validator,
        [FromKeyedServices(UnitOfWorkContext.Species)]IUnitOfWork unitOfWork,
        ILogger<CreateSpeciesCommandHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
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

        var species = new Domain.Species(name);

        var result = await _speciesRepository.AddAsync(species, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("The species was created with the ID: {speciesId}", result);

        return result;
    }
}