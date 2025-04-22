using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.DTOs.Volunteer;

namespace PetFamily.Volunteers.Contracts.Requests;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    string? Description,
    byte WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    IEnumerable<RequisiteDto>? Requisites);