using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Files;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Providers;
using PetFamily.Core.Models;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.HardDeletePet;

public class HardDeletePetHandler : ICommandHandler<Guid, HardDeletePetCommand>
{
    
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IValidator<HardDeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HardDeletePetHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public HardDeletePetHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IValidator<HardDeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<HardDeletePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        HardDeletePetCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting hard-deletion pet with id = {PetId}", command.PetId);

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Failed hard-deletion pet");
                return validationResult.ToErrorList();
            }

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Failed hard-deletion pet");
                return (ErrorList)volunteerResult.Error;
            }

            var petResult = volunteerResult.Value.GetPetById(command.PetId);
            if (petResult.IsFailure)
            {
                _logger.LogWarning("Failed hard-deletion pet");
                return (ErrorList)petResult.Error;
            }

            var petPhotos = petResult.Value.PetPhotos;
            foreach (var petPhoto in petPhotos)
            {
                var fileIdentifier = new FileIdentifier(petPhoto.PhotoPath, BUCKET_NAME);
                
                await _fileProvider.RemoveFileAsync(fileIdentifier, cancellationToken);
            }
            
            volunteerResult.Value.DeletePet(petResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("Successfully hard-deletion pet with id = {PetId}", command.PetId);
            
            return petResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed hard-deletion pet");
            
            transaction.Rollback();
            
            return (ErrorList)Error.Failure("delete.files.error",
                $"deletion files to pet with id = {command.PetId} failed");
        }
    }
}