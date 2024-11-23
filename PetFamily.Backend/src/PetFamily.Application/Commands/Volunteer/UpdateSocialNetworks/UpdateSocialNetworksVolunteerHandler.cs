using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksVolunteerHandler
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

        var socialNetworks = new SocialNetworksShell(command.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value));

        volunteerResult.Value.UpdateSocialNetwork(socialNetworks);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

        transaction.Commit();

        return volunteerResult.Value.Id.Value;
    }
}