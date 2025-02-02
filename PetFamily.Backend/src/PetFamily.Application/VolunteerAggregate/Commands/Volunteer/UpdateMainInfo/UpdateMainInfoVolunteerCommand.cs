using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.UpdateMainInfo;

public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber) : ICommand;