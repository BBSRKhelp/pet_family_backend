using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Feature.Commands.Login;
using PetFamily.Accounts.Application.Feature.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromServices] RegisterUserHandler handler,
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
        [FromServices] LoginHandler handler,
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }
}