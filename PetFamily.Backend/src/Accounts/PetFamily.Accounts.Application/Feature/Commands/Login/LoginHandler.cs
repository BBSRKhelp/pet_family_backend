using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Feature.Commands.Login;

public class LoginHandler : ICommandHandler<string, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager, 
        ITokenProvider tokenProvider,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
    
    public async Task<Result<string, ErrorList>> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("logging in for {Email}...", command.Email);
        
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            _logger.LogInformation("User with email: {Email} not found", command.Email);
            return (ErrorList)Errors.General.NotFound("user");
        }
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
        {
            _logger.LogInformation("User with email: {Email} not confirmed", command.Email);
            return (ErrorList)Errors.User.InvalidCredentials();
        }
        
        var token = _tokenProvider.GenerateAccessToken(user);

        _logger.LogInformation("User with email: {Email} logged in", command.Email);
        
        return token;
    }
}