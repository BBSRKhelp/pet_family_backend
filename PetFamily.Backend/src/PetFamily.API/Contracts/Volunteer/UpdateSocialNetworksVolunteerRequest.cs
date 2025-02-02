using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateSocialNetworks;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, SocialNetworks);
    }
}