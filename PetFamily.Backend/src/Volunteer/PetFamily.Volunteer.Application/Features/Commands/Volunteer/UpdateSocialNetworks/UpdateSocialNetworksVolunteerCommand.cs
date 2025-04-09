using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateSocialNetworks;

public record UpdateSocialNetworksVolunteerCommand(Guid Id, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;