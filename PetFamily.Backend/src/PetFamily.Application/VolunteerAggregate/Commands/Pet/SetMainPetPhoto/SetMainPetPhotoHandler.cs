using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Core.Extensions;
using PetFamily.Core.Interfaces.Abstractions;
using PetFamily.Core.Interfaces.Database;
using PetFamily.Core.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SetMainPetPhoto;

public class SetMainPetPhotoHandler : ICommandHandler<Guid, SetMainPetPhotoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SetMainPetPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetMainPetPhotoHandler> _logger;

    public SetMainPetPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<SetMainPetPhotoCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<SetMainPetPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        SetMainPetPhotoCommand command, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Setting main pet photo with id = {PetId}", command.PetId);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Set main pet photo failed");
            return validationResult.ToErrorList();
        }
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Set main pet photo failed");
            return (ErrorList)volunteerResult.Error;
        }
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
        {
            _logger.LogWarning("Set main pet photo failed");
            return (ErrorList)petResult.Error;
        }

        var photoPath = PhotoPath.Create(command.PhotoPath).Value;
        
        var result = petResult.Value.SetMainPhoto(photoPath);
        if (result.IsFailure)
        {
            _logger.LogWarning("Set main pet photo failed");
            return (ErrorList)result.Error;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Set main pet photo success with id = {PetId}", command.PetId);
        
        return petResult.Value.Id.Value;
    }
}