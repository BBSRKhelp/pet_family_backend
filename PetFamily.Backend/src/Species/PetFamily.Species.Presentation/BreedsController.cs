using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Species.Application.Feature.Queries.GetBreedsByIdSpecies;
using PetFamily.Species.Contracts.DTOs;
using PetFamily.Species.Contracts.Requests;

namespace PetFamily.Species.Presentation;

[ApiController]
[Route("[controller]")]
public class BreedsController : ControllerBase
{
    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<ActionResult<PagedList<BreedDto>>> GetBreedAsync(
        [FromServices] GetBreedsByIdSpeciesHandler speciesHandler,
        [FromServices] IMapper mapper,
        [FromQuery] GetBreedsByIdSpeciesRequest request,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var query = mapper.Map<GetBreedsByIdSpeciesQuery>(request, opts =>
        {
            opts.Items["SpeciesId"] = speciesId;
        });
        
        var result = await speciesHandler.HandleAsync(query, cancellationToken);
        
        return result.ToResponse();
    }
}