using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Species.Application.Feature.Queries.GetBreedsByIdSpecies;
using PetFamily.Species.Contracts.DTOs;
using PetFamily.Species.Presentation.Breeds.Requests;

namespace PetFamily.Species.Presentation.Breeds;

[ApiController]
[Route("[controller]")]
public class BreedsController : ControllerBase
{
    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<ActionResult<PagedList<BreedDto>>> GetBreedAsync(
        [FromServices] GetBreedsByIdSpeciesHandler speciesHandler,
        [FromQuery] GetBreedsByIdSpeciesRequest request,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery(speciesId);
        var result = await speciesHandler.HandleAsync(query, cancellationToken);
        
        return result.ToResponse();
    }
}