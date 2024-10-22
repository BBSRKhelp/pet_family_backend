using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.Application.Commands.Volunteer.Create;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] VolunteerCreateHandler handler,
        [FromServices] VolunteerCreateCommandValidator validator, 
        [FromBody] VolunteerCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }
}