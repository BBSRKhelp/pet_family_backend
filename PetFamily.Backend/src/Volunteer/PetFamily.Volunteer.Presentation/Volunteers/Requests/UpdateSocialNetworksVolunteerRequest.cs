using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateSocialNetworksVolunteerCommand(id, SocialNetworks);
    }
}