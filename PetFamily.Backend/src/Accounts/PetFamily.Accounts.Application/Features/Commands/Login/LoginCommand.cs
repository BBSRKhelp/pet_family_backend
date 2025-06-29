using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Features.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand;