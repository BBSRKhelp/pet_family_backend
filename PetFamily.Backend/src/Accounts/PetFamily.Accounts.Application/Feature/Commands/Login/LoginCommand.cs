using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Feature.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;