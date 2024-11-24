using PetFamily.Application.Dtos;

namespace PetFamily.Application.Commands.Volunteer.UpdateMainInfo;

public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber);