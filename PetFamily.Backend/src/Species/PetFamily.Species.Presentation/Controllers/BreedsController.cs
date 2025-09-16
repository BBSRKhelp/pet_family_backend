using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Species.Application.Features.Queries.GetBreedsByIdSpecies;
using PetFamily.Species.Contracts.DTOs;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BreedsController : ControllerBase
{
    [Permission(Permissions.Breeds.GET)]
    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<ActionResult<PagedList<BreedDto>>> GetBreedAsync(
        [FromServices] GetBreedsByIdSpeciesQueryHandler speciesQueryHandler,
        [FromQuery] GetBreedsByIdSpeciesRequest request,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery(speciesId);

        var result = await speciesQueryHandler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }
}