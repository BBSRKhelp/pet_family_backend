using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Commands.Volunteer.UpdateMainInfo;

public class VolunteerUpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateMainInfoHandler> _logger;

    public VolunteerUpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating Volunteer");

        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var email = Email.Create(command.Email).Value;
        var description = Description.Create(command.Description).Value;
        var workExperience = WorkExperience.Create(command.WorkExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var volunteerResultByEmail = await _volunteersRepository.GetByEmailAsync(email, cancellationToken);
        if (volunteerResultByEmail.IsSuccess)
            return Errors.General.IsExisted(nameof(email));

        var volunteerResultByPhone = await _volunteersRepository.GetByPhoneAsync(phoneNumber, cancellationToken);
        if (volunteerResultByPhone.IsSuccess)
            return Errors.General.IsExisted(nameof(phoneNumber));

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            email,
            description,
            workExperience,
            phoneNumber);

        var result = await _volunteersRepository.SaveChangesAsync(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", result);
        
        return result;
    }
}