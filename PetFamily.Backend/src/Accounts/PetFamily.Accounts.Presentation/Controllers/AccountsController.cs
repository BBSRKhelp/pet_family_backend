using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Features.Commands.Login;
using PetFamily.Accounts.Application.Features.Commands.RefreshToken;
using PetFamily.Accounts.Application.Features.Commands.Register;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromServices] RegisterCommandHandler commandHandler,
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("authorization")]
    public async Task<ActionResult<string>> Login(
        [FromServices] LoginCommandHandler commandHandler,
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await commandHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());
        
        return result.Value.AccessToken;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<string>> Refresh(
        [FromBody] RefreshTokensRequest request,
        [FromServices] RefreshTokensCommandHandler commandHandler,
        CancellationToken cancellationToken = default)
    { 
        var command = request.ToCommand();
        
        var result = await commandHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());
        
        return result.Value.AccessToken;
    }
}