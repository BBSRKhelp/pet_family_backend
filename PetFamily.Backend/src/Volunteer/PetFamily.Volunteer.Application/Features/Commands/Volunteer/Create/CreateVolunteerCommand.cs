using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites) : ICommand;