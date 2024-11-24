using PetFamily.Application.Dtos;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public record UpdateSocialNetworksVolunteerCommand(Guid Id, IEnumerable<SocialNetworkDto> SocialNetworks);