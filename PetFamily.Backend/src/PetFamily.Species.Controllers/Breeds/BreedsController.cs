using PetFamily.Core.Models;
using PetFamily.Species.Application.DTOs.Read;
using PetFamily.Species.Application.Queries.GetBreedsByIdSpecies;
using PetFamily.Species.Controllers.Breeds.Requests;

namespace PetFamily.Species.Controllers.Breeds;

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