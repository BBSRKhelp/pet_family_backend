using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Models;
using PetFamily.Framework;
using PetFamily.Framework.Processors;
using PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;
using PetFamily.Volunteer.Application.Features.Commands.Pet.ChangePetsPosition;
using PetFamily.Volunteer.Application.Features.Commands.Pet.HardDeletePet;
using PetFamily.Volunteer.Application.Features.Commands.Pet.SetMainPetPhoto;
using PetFamily.Volunteer.Application.Features.Commands.Pet.SoftDeletePet;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdateMainPetInfo;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdatePetStatus;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UploadFilesToPet;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.Create;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.Delete;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateRequisites;
using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Volunteer.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;
using PetFamily.Volunteer.Application.Features.Queries.Volunteer.GetVolunteerById;
using PetFamily.Volunteer.Contracts.DTOs;
using PetFamily.Volunteer.Contracts.Requests;

namespace PetFamily.Volunteer.Presentation.Controllers;

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
        [FromServices] UpdateMainVolunteerInfoHandler handler,
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
        [FromBody] AddPetRequest request,
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

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainPetInfoAsync(
        [FromServices] UpdateMainPetInfoHandler handler,
        [FromBody] UpdateMainPetInfoRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdatePetStatusAsync(
        [FromServices] UpdatePetStatusHandler handler,
        [FromBody] UpdatePetStatusRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/soft")]
    public async Task<ActionResult<Guid>> SoftDeletePetAsync(
        [FromServices] SoftDeletePetHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = new SoftDeletePetCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDeletePetAsync(
        [FromServices] HardDeletePetHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = new HardDeletePetCommand(volunteerId, petId);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-photo")]
    public async Task<ActionResult<Guid>> SetMainPetPhotoAsync(
        [FromServices] SetMainPetPhotoHandler handler,
        [FromBody] SetMainPetPhotoRequest request,
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