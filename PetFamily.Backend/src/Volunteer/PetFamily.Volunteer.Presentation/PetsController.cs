using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Volunteer.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Volunteer.Application.Features.Queries.Pet.GetPetById;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.Requests;

namespace PetFamily.Volunteer.Presentation;

[ApiController]
[Route("[controller]")]
public class PetsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedList<PetDto>>> GetPetsAsync(
        [FromServices] GetFilteredPetsWithPaginationHandlerDapper handler,
        [FromServices] IMapper mapper,
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = mapper.Map<GetFilteredPetsWithPaginationQuery>(request);
        
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
