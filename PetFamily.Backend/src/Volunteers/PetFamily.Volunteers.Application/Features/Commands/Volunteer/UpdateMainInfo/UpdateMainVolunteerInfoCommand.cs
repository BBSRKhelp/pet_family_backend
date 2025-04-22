using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateMainInfo;

public record UpdateMainVolunteerInfoCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber) : ICommand;