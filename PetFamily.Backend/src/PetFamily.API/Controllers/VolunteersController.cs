using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Volunteer;
using PetFamily.API.Extensions;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Commands.Volunteer.Delete;
using PetFamily.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Domain.Shared.Models;

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
        [FromRoute] Guid id)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command);
        
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.HandleAsync(command);
        
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
}