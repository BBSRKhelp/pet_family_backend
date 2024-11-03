using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public class VolunteerUpdateSocialNetworksHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<VolunteerUpdateSocialNetworksHandler> _logger;

    public VolunteerUpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<VolunteerUpdateSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerUpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating the volunteer's social networks");

        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var socialNetworks = new SocialNetworkShell(command.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value));
        
        volunteerResult.Value.UpdateSocialNetwork(socialNetworks);
        
        var result = await _volunteersRepository.SaveChangesAsync(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", result);

        return result;
    }
}