using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Application.Dto;

namespace PetFamily.API.Contracts.Volunteer;

public record VolunteerUpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public VolunteerUpdateSocialNetworksCommand ToCommand(Guid id)
    {
        return new VolunteerUpdateSocialNetworksCommand(id, SocialNetworks);
    }
}