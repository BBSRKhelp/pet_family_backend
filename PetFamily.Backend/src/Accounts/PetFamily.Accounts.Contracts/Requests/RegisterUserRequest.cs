using PetFamily.Accounts.Contracts.DTOs;

namespace PetFamily.Accounts.Contracts.Requests;

public record RegisterUserRequest(
    string UserName,
    string FirstName,
    string LastName,
    string? Patronymic,
    string Email,
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    string Password);