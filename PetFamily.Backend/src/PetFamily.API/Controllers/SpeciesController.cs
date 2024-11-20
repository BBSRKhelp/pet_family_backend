using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Breed;
using PetFamily.API.Contracts.Species;
using PetFamily.API.Extensions;
using PetFamily.Application.Commands.Species.AddBreed;
using PetFamily.Application.Commands.Species.Create;
using PetFamily.Domain.SpeciesAggregate;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
       [FromServices] SpeciesCreateHandler handler,
       [FromServices] SpeciesCreateCommandValidator commandValidator,
       [FromBody] SpeciesCreateRequest request,
       CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();
        
        var validationResult = await commandValidator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);
        
        return result.ToResponse();
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<ActionResult<Guid>> AddBreedAsync(
        [FromServices] BreedCreateHandler handler,
        [FromServices] BreedCreateCommandValidator validator,
        [FromBody] BreedCreateRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.HandleAsync(command, cancellationToken);
        
        return result.ToResponse();
    }
}