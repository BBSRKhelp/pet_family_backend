using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.Create;

public class VolunteerCreateHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerCreateHandler> _logger;
    public VolunteerCreateHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerCreateHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating Volunteer");

        var fullname = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var email = Email.Create(command.Email).Value;

        var description = Description.Create(command.Description).Value;

        var workExperience = WorkExperience.Create(command.WorkExperience).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var socialNetwork = command.SocialNetworks
            ?.Select(x => SocialNetwork.Create(x.Title, x.Url).Value) ?? [];
        var socialNetworks = new SocialNetworkShell(socialNetwork);

        var requisite = command.Requisites
            ?.Select(x => Requisite.Create(x.Title, x.Description).Value) ?? [];
        var requisites = new RequisitesShell(requisite);

        var volunteerEmail = await _volunteersRepository.GetByEmailAsync(email, cancellationToken);
        if (volunteerEmail.IsSuccess)
            return Errors.General.IsExisted(nameof(email));

        var volunteerPhone = await _volunteersRepository.GetByPhoneAsync(phoneNumber, cancellationToken);
        if (volunteerPhone.IsSuccess)
            return Errors.General.IsExisted(nameof(phoneNumber));

        var volunteer = new Domain
            .VolunteerAggregate
            .Volunteer(
                fullname,
                email,
                description,
                workExperience,
                phoneNumber,
                socialNetworks,
                requisites);
        
        var result = await _volunteersRepository.AddAsync(volunteer, cancellationToken);
        
        _logger.LogInformation("The volunteer was created with the ID: {volunteerId}", result);
        
        return result;
    }
}