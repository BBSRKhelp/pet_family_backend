using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.UpdateMainInfo;

public class UpdateMainInfoVolunteerHandler : ICommandHandler<Guid, UpdateMainInfoVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateMainInfoVolunteerCommand> _validator;
    private readonly ILogger<UpdateMainInfoVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMainInfoVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateMainInfoVolunteerCommand> validator,
        ILogger<UpdateMainInfoVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateMainInfoVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating main info volunteer");
        
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

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

        transaction.Commit();

        return volunteerResult.Value.Id.Value;
    }
}