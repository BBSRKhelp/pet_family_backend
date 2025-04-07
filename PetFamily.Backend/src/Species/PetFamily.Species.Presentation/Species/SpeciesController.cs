using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Species.Application.Feature.Commands.Breed.AddBreed;
using PetFamily.Species.Application.Feature.Commands.Breed.DeleteBreed;
using PetFamily.Species.Application.Feature.Commands.Species.Create;
using PetFamily.Species.Application.Feature.Commands.Species.Delete;
using PetFamily.Species.Application.Feature.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.DTOs;
using PetFamily.Species.Presentation.Species.Requests;

namespace PetFamily.Species.Presentation.Species;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
       [FromServices] CreateSpeciesHandler handler,
       [FromBody] CreateSpeciesRequest request,
       CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await handler.HandleAsync(command, cancellationToken);
        
        return result.ToResponse();
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<ActionResult<Guid>> AddBreedAsync(
        [FromServices] AddBreedHandler handler,
        [FromBody] CreateBreedRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        
        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteAsync(
        [FromServices] DeleteSpeciesHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(id);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreedAsync(
        [FromServices] DeleteBreedHandler handler,
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<SpeciesDto>>> GetAsync(
        [FromServices] GetFilteredSpeciesWithPaginationHandler handler,
        [FromQuery] GetFilteredSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }
}