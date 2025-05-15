using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteers.Contracts.Requests;

public record UpdateSocialNetworksVolunteerRequest(IEnumerable<SocialNetworkDto> SocialNetworks);