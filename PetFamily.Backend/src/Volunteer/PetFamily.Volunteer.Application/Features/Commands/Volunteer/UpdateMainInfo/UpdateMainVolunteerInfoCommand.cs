using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateMainInfo;

public record UpdateMainVolunteerInfoCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber) : ICommand;