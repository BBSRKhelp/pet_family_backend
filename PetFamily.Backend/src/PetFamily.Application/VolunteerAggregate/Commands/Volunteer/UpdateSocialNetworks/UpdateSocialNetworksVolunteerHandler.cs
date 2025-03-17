using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Core.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksVolunteerHandler : ICommandHandler<Guid, UpdateSocialNetworksVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateSocialNetworksVolunteerCommand> _validator;
    private readonly ILogger<UpdateSocialNetworksVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSocialNetworksVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateSocialNetworksVolunteerCommand> validator,
        ILogger<UpdateSocialNetworksVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateSocialNetworksVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating the volunteer's social networks");
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Volunteer update failed");
            return (ErrorList)volunteerResult.Error;
        }

        var socialNetworks = command.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value)
            .ToArray();

        volunteerResult.Value.UpdateSocialNetwork(socialNetworks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

        return volunteerResult.Value.Id.Value;
    }
}