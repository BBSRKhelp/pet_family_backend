using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Commands.Volunteer.UpdateMainInfo;

public class VolunteerUpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteerUpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateMainInfoHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Updating main info volunteer");

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Volunteer update failed");
                return volunteerResult.Error;
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
                return Errors.General.IsExisted(nameof(email));
            }

            var volunteerResultByPhone = await _volunteersRepository.GetByPhoneAsync(phoneNumber, cancellationToken);
            if (volunteerResult.Value.PhoneNumber != phoneNumber && volunteerResultByPhone.IsSuccess)
            {
                _logger.LogWarning("Volunteer update failed");
                return Errors.General.IsExisted(nameof(phoneNumber));
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update main info volunteer");
            
            transaction.Rollback();
            
            return Error.Failure("update.main.info.volunteer", "Failed to update main info volunteer");
        }
    }
}