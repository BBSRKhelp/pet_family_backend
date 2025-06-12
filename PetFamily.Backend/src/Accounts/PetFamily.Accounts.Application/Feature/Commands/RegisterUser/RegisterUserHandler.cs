using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Feature.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager, 
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Register user account");
        
        var user = User.CreateUser(command.UserName, command.Email);
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User: {UserName} created a new account with password", command.UserName);
            
            await _userManager.AddToRoleAsync(user, "participant");

            return UnitResult.Success<ErrorList>();
        }

        var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description));
        
        _logger.LogInformation("User: {UserName} could not be created", command.UserName);
        
        return new ErrorList(errors);
    }
}