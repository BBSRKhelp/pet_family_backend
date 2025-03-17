using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Core.Models;
using PetFamily.Core.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateMainInfo;

public class UpdateMainVolunteerInfoHandler : ICommandHandler<Guid, UpdateMainVolunteerInfoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateMainVolunteerInfoCommand> _validator;
    private readonly ILogger<UpdateMainVolunteerInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMainVolunteerInfoHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateMainVolunteerInfoCommand> validator,
        ILogger<UpdateMainVolunteerInfoHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateMainVolunteerInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating main info volunteer with id = {VolunteerId}", command.Id);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Volunteer update failed");
            return (ErrorList)volunteerResult.Error;
        }

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var email = Email.Create(command.Email).Value;
        var description = Description.Create(command.Description).Value;
        var workExperience = WorkExperience.Create(command.WorkExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var volunteerResultByEmail = await _volunteersRepository.GetByEmailAsync(email, cancellationToken);
        if (volunteerResult.Value.Email != email && volunteerResultByEmail.IsSuccess)
        {
            _logger.LogWarning("Volunteer update failed");
            return (ErrorList)Errors.General.IsExisted(nameof(email));
        }

        var volunteerResultByPhone = await _volunteersRepository.GetByPhoneAsync(phoneNumber, cancellationToken);
        if (volunteerResult.Value.PhoneNumber != phoneNumber && volunteerResultByPhone.IsSuccess)
        {
            _logger.LogWarning("Volunteer update failed");
            return (ErrorList)Errors.General.IsExisted(nameof(phoneNumber));
        }

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            workExperience,
            phoneNumber);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

        return volunteerResult.Value.Id.Value;
    }
}