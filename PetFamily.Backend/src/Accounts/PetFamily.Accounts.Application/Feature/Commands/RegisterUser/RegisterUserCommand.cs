using PetFamily.Accounts.Contracts.DTOs;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;

namespace PetFamily.Accounts.Application.Feature.Commands.RegisterUser;

public record RegisterUserCommand(
    string UserName,
    FullNameDto FullName,
    string Email, 
    IEnumerable<SocialNetworkDto>? SocialNetworks,
    string Password) : ICommand;