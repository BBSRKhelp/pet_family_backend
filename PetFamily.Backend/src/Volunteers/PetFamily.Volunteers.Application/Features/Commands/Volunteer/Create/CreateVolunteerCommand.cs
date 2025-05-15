using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites) : ICommand;