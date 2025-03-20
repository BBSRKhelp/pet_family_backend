using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateMainInfo;

public record UpdateMainVolunteerInfoCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber) : ICommand;