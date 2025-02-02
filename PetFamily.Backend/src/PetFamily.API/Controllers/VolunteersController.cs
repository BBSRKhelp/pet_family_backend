using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Pet;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.DTOs.Read;
using PetFamily.Application.Models;
using PetFamily.Application.VolunteerAggregate.Commands.AddPet;
using PetFamily.Application.VolunteerAggregate.Commands.ChangePetsPosition;
using PetFamily.Application.VolunteerAggregate.Commands.Create;
using PetFamily.Application.VolunteerAggregate.Commands.Delete;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateMainInfo;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateRequisites;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateSocialNetworks;
using PetFamily.Application.VolunteerAggregate.Commands.UploadFilesToPet;
using PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;
using PetFamily.Application.VolunteerAggregate.Queries.GetVolunteerById;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfoAsync(
        [FromServices] UpdateMainInfoVolunteerHandler handler,
        [FromBody] UpdateMainInfoVolunteerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisitesAsync(
        [FromServices] UpdateRequisitesVolunteerHandler handler,
        [FromBody] UpdateRequisitesVolunteerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworksAsync(
        [FromServices] UpdateSocialNetworksVolunteerHandler handler,
        [FromBody] UpdateSocialNetworksVolunteerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteAsync(
        [FromServices] DeleteVolunteerHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{id:guid}/pets")]
    public async Task<ActionResult<Guid>> AddPetAsync(
        [FromServices] AddPetHandler handler,
        [FromBody] CreatePetRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
    public async Task<ActionResult<Guid>> UploadFilesToPetAsync(
        [FromServices] UploadFilesToPetHandler handler,
        [FromForm] IFormFileCollection files,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadFilesToPetCommand(volunteerId, petId, fileDtos);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/position")]
    public async Task<ActionResult> ChangePetsPosition(
        [FromServices] ChangePetsPositionHandler handler,
        [FromBody] ChangePetsPositionRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<VolunteerDto>>> GetAsync(
        [FromServices] GetFilteredVolunteersWithPaginationHandlerDapper handler,
        [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();

        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VolunteerDto>> GetByIdAsync(
        [FromServices] GetVolunteerByIdHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerByIdQuery(id);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        
        return result.ToResponse();
    }
}