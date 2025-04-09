using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteer.Application.Interfaces;
using PetFamily.Volunteer.Application.Validation;
using PetFamily.Volunteer.Domain.ValueObjects;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly SpeciesAndBreedValidator _speciesAndBreedValidator;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        SpeciesAndBreedValidator speciesAndBreedValidator,
        IValidator<AddPetCommand> validator,
        [FromKeyedServices(UnitOfWorkContext.Volunteer)]IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _speciesAndBreedValidator = speciesAndBreedValidator;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating Pet");

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Pet creation failed");
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogError("Pet creation failed");
            return (ErrorList)volunteerResult.Error;
        }

        var name = Name.Create(command.Name).Value;

        var description = Description.Create(command.Description).Value;

        var appearanceDetails = AppearanceDetails.Create(
            command.AppearanceDetails.Coloration,
            command.AppearanceDetails.Weight,
            command.AppearanceDetails.Height).Value;

        var healthDetails = HealthDetails.Create(
            command.HealthDetails.HealthInformation,
            command.HealthDetails.IsCastrated,
            command.HealthDetails.IsVaccinated).Value;

        var address = Address.Create(
            command.Address.Country,
            command.Address.City,
            command.Address.Street,
            command.Address.PostalCode).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var requisites = command.Requisites
            ?.Select(x => Requisite.Create(x.Title, x.Description).Value).ToArray() ?? [];

        var speciesAndBreedExists = await _speciesAndBreedValidator
            .IsExist(command.BreedAndSpeciesId, cancellationToken);
        if (speciesAndBreedExists.IsFailure)
        {
            _logger.LogError("Pet creation failed");
            return speciesAndBreedExists.Error;
        }

        var breedAndSpeciesId = BreedAndSpeciesId.Create(
            command.BreedAndSpeciesId.SpeciesId,
            command.BreedAndSpeciesId.BreedId).Value;

        var pet = new Domain.Entities.Pet(
            name,
            description,
            appearanceDetails,
            healthDetails,
            address,
            phoneNumber,
            command.BirthDate,
            command.Status,
            requisites,
            breedAndSpeciesId);

        var result = volunteerResult.Value.AddPet(pet);
        if (result.IsFailure)
            return (ErrorList)result.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("The pet was created and added to the database with an Id: {PetId}", pet.Id.Value);

        return pet.Id.Value;
    }
}