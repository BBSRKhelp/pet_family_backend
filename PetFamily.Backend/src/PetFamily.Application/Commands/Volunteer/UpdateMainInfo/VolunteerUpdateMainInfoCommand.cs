using PetFamily.Application.Dto;

namespace PetFamily.Application.Commands.Volunteer.UpdateMainInfo;

public record VolunteerUpdateMainInfoCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber);