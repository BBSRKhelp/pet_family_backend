using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksVolunteerCommand(Guid Id, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;