using PetFamily.Application.Dtos;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateSocialNetworks;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, SocialNetworks);
    }
}