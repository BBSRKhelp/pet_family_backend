using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites) : ICommand;