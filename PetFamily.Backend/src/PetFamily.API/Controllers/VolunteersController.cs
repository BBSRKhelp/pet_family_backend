using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Pet;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Commands.Volunteer.AddPet;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Commands.Volunteer.Delete;
using PetFamily.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Application.Commands.Volunteer.UploadFilesToPet;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromServices] VolunteerCreateHandler handler,
        [FromServices] VolunteerCreateCommandValidator validator,
        [FromBody] VolunteerCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand();

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfoAsync(
        [FromServices] VolunteerUpdateMainInfoHandler handler,
        [FromServices] VolunteerUpdateMainInfoCommandValidator validator,
        [FromBody] VolunteerUpdateMainInfoRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisitesAsync(
        [FromServices] VolunteerUpdateRequisitesHandler handler,
        [FromServices] VolunteerUpdateRequisitesCommandValidator validator,
        [FromBody] VolunteerUpdateRequisitesRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworksAsync(
        [FromServices] VolunteerUpdateSocialNetworksHandler handler,
        [FromServices] VolunteerUpdateSocialNetworksCommandValidator validator,
        [FromBody] VolunteerUpdateSocialNetworksRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteAsync(
        [FromServices] VolunteerDeleteHandler handler,
        [FromServices] VolunteerDeleteCommandValidator validator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new VolunteerDeleteCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{id:guid}/pets")]
    public async Task<ActionResult<Guid>> AddAsync(
        [FromServices] PetCreateHandler handler,
        [FromServices] PetCreateCommandValidator validator,
        [FromBody] PetCreateRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{volunteerId:guid}/pets{petId:guid}")]
    public async Task<ActionResult<Guid>> UploadFilesToPetAsync(
        [FromServices] UploadFilesToPetHandler handler,
        [FromServices] UploadFilesToPetCommandValidator validator,
        [FromForm] IFormFileCollection files,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();
        var fileDtos = fileProcessor.Process(files);

        var command = new UploadFilesToPetCommand(volunteerId, petId, fileDtos);
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.HandleAsync(command, cancellationToken);
        
        return result.ToResponse();
    }
}