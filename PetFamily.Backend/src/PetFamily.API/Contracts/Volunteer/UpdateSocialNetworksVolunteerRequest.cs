using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Application.Dtos;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, SocialNetworks);
    }
}