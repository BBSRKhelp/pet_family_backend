using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Species.Application.Features.Commands.Breed.AddBreed;
using PetFamily.Species.Application.Features.Commands.Breed.DeleteBreed;
using PetFamily.Species.Application.Features.Commands.Species.Create;
using PetFamily.Species.Application.Features.Commands.Species.Delete;
using PetFamily.Species.Application.Features.Queries.GetFilteredSpeciesWithPagination;
using PetFamily.Species.Contracts.DTOs;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [Permission(Permissions.Species.CREATE)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromServices] CreateSpeciesCommandHandler commandHandler,
        [FromBody] CreateSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [Permission(Permissions.Species.GET)]
    [HttpGet]
    public async Task<ActionResult<PagedList<SpeciesDto>>> GetAsync(
        [FromServices] GetFilteredSpeciesWithPaginationQueryHandler queryHandler,
        [FromQuery] GetFilteredSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }

    [Permission(Permissions.Species.DELETE)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteAsync(
        [FromServices] DeleteSpeciesCommandHandler commandHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(id);

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [Permission(Permissions.Species.BREED_ADD)]
    [HttpPost("{id:guid}/breeds")]
    public async Task<ActionResult<Guid>> AddBreedAsync(
        [FromServices] AddBreedCommandHandler commandHandler,
        [FromBody] AddBreedRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [Permission(Permissions.Species.BREED_DELETE)]
    [HttpDelete("{speciesId:guid}/breeds/{breedId:guid}")]
    public async Task<ActionResult<Guid>> DeleteBreedAsync(
        [FromServices] DeleteBreedCommandHandler commandHandler,
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }
}