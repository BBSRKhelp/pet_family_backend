using PetFamily.Application.Dto;

namespace PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

public record VolunteerUpdateSocialNetworksCommand(Guid Id, IEnumerable<SocialNetworkDto> SocialNetworks);