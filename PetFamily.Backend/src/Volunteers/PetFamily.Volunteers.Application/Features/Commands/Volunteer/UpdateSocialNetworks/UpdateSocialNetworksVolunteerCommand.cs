using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateSocialNetworks;

public record UpdateSocialNetworksVolunteerCommand(Guid Id, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;