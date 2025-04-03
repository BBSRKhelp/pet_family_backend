using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Enums;
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

namespace PetFamily.Volunteer.Presentation;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromServices] CreateVolunteerHandler handler,
        [FromServices] IMapper mapper,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = mapper.Map<CreateVolunteerCommand>(request);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfoAsync(
        [FromServices] UpdateMainVolunteerInfoHandler handler,
        [FromServices] IMapper mapper,
        [FromBody] UpdateMainInfoVolunteerRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = mapper.Map<UpdateMainVolunteerInfoCommand>(request, opts =>
        {
            opts.Items["Id"] = id;
        });

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
        var command = new UpdateRequisitesVolunteerCommand(id, request.Requisite);

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
        var command = new UpdateSocialNetworksVolunteerCommand(id, request.SocialNetworks);

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
        [FromServices] IMapper mapper,
        [FromBody] CreatePetRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = mapper.Map<AddPetCommand>(request, opts =>
        {
            opts.Items["Id"] = id;
        });

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
        var command = new ChangePetsPositionCommand(volunteerId, petId, request.NewPosition);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdatePetMainInfoAsync(
        [FromServices] UpdateMainPetInfoHandler handler,
        [FromServices] IMapper mapper,
        [FromBody] UpdatePetMainInfoRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        var command = mapper.Map<UpdateMainPetInfoCommand>(request, opts =>
        {
            opts.Items["VolunteerId"] = volunteerId;
            opts.Items["PetId"] = petId;
        });

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
        var status = Enum.TryParse(request.Status, true, out Status statusResult)
            ? statusResult 
            : Status.Unknown;   
        
        var command = new UpdatePetStatusCommand(volunteerId, petId, status);

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
        var command = new SetMainPetPhotoCommand(volunteerId, petId, request.PhotoPath);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<VolunteerDto>>> GetAsync(
        [FromServices] GetFilteredVolunteersWithPaginationHandlerDapper handler,
        [FromServices] IMapper mapper,
        [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = mapper.Map<GetFilteredVolunteersWithPaginationQuery>(request);

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