using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.DTOs.Read;
using PetFamily.Application.VolunteerAggregate.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Application.VolunteerAggregate.Queries.Pet.GetPetById;
using PetFamily.Core.Models;

namespace PetFamily.API.Controllers.Pets;

[ApiController]
[Route("[controller]")]
public class PetsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedList<PetDto>>> GetPetsAsync(
        [FromServices] GetFilteredPetsWithPaginationHandlerDapper handler,
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PetDto>> GetPetByIdAsync(
        [FromServices] GetPetByIdHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(id);
        
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }
}
