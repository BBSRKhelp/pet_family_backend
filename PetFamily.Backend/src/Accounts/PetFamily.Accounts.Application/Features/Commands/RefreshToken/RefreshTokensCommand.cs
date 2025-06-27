using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Features.Commands.RefreshToken;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;