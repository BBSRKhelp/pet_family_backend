using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public class VolunteerUpdateSocialNetworksHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateSocialNetworksHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteerUpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateSocialNetworksHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("Updating the volunteer's social networks");

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Volunteer update failed");
                return volunteerResult.Error;
            }

            var socialNetworks = new SocialNetworksShell(command.SocialNetworks
                .Select(x => SocialNetwork.Create(x.Title, x.Url).Value));

            volunteerResult.Value.UpdateSocialNetwork(socialNetworks);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

            transaction.Commit();

            return volunteerResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update volunteer's social networks");

            transaction.Rollback();

            return Error.Failure("update.social.networks.volunteer", "Failed to update volunteer's social networks");
        }
    }
}