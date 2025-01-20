using PetFamily.Application.Dtos;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites) : ICommand;