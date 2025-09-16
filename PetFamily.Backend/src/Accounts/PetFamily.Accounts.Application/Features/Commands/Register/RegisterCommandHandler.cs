using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Features.Commands.Register;

public class RegisterCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IParticipantAccountManager participantAccountManager,
    IValidator<RegisterCommand> validator,
    [FromKeyedServices(UnitOfWorkContext.Accounts)]
    IUnitOfWork unitOfWork,
    ILogger<RegisterCommandHandler> logger)
    : ICommandHandler<RegisterCommand>
{
    public async Task<UnitResult<ErrorList>> HandleAsync(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Register user account");

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogInformation("Validation failed for user registration");
            return validationResult.ToErrorList();
        }

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Patronymic).Value;

        var socialNetworks = command.SocialNetworks
            ?.Select(x => SocialNetwork.Create(x.Title, x.Url).Value)
            .ToArray() ?? [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var role = await roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
            if (role is null)
            {
                logger.LogError("Role not found");
                transaction.Rollback();
                return Errors.General.NotFound(nameof(role)).ToErrorList();
            }

            var participantUserResult = User.CreateParticipant(
                command.UserName,
                fullName,
                command.Email,
                null,
                socialNetworks,
                role);
            if (participantUserResult.IsFailure)
            {
                logger.LogError("Create participant failed");
                transaction.Rollback();
                return Errors.General.IsInvalid("user").ToErrorList();
            }

            var createResult = await userManager.CreateAsync(participantUserResult.Value, command.Password);
            if (!createResult.Succeeded)
            {
                logger.LogInformation("User: {UserName} could not be created", command.UserName);
                transaction.Rollback();

                var errors = createResult.Errors.Select(e => Error.Failure(e.Code, e.Description));
                return new ErrorList(errors);
            }

            var participantAccount = new ParticipantAccount(participantUserResult.Value);
            await participantAccountManager.CreateParticipantAccountAsync(participantAccount, cancellationToken);

            transaction.Commit();
            logger.LogInformation("User {UserName} registered successfully", command.UserName);

            return UnitResult.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to register user {UserName}", command.UserName);
            transaction.Rollback();
            throw;
        }
    }
}