using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Contracts.Requests;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    /*public UpdateSocialNetworksVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, SocialNetworks);
    }*/
}