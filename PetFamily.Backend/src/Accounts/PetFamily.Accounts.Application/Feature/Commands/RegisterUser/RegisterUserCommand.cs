using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Feature.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email, 
    string UserName, 
    string Password) : ICommand;