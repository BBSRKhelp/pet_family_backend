using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.Volunteers.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Volunteers.Application.Features.Queries.Pet.GetPetById;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PetsController : ControllerBase
{
    [Permission(Permissions.Pets.GET)]
    [HttpGet]
    public async Task<ActionResult<PagedList<PetDto>>> GetPetsAsync(
        [FromServices] GetFilteredPetsWithPaginationQueryHandlerDapper queryHandler,
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }

    [Permission(Permissions.Pets.GET)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PetDto>> GetPetByIdAsync(
        [FromServices] GetPetByIdQueryHandler queryHandler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(id);

        var result = await queryHandler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }
}