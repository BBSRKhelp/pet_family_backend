using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public class UpdateRequisitesVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateRequisitesVolunteerCommand> _validator;
    private readonly ILogger<UpdateRequisitesVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRequisitesVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateRequisitesVolunteerCommand> validator,
        ILogger<UpdateRequisitesVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UpdateRequisitesVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating the volunteer's requisites");

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Volunteer update failed");
            return (ErrorList)volunteerResult.Error;
        }

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Title, r.Description).Value);

        volunteerResult.Value.UpdateRequisite(new RequisitesShell(requisites));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer with Id = {VolunteerId} has been update", command.Id);

        transaction.Commit();

        return volunteerResult.Value.Id.Value;
    }
}