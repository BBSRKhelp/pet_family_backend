using PetFamily.Application.Dtos;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.UpdateMainInfo;

public record UpdateMainInfoVolunteerCommand(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber) : ICommand;