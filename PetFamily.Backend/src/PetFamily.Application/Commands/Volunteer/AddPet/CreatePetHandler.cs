using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.AddPet;

public class CreatePetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<CreatePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePetHandler> _logger;

    public CreatePetHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<CreatePetCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<CreatePetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreatePetCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating Pet");

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Pet creation failed");
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            _logger.LogWarning("Pet creation failed");
            return (ErrorList)volunteerResult.Error;
        }

        var name = Name.Create(command.Name).Value;

        var description = Description.Create(command.Description).Value;

        var appearanceDetails = AppearanceDetails.Create(
            command.AppearanceDetails.Colouration,
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
            command.Address.Postalcode).Value;

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var requisites = new RequisitesShell(command.Requisites
            ?.Select(x => Requisite.Create(x.Title, x.Description).Value) ?? []);

        var speciesId = command.BreedAndSpeciesId.SpeciesId;

        var speciesResult = await _speciesRepository.GetByIdAsync(speciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            _logger.LogWarning("Pet creation failed");
            return (ErrorList)speciesResult.Error;
        }

        var breedId = command.BreedAndSpeciesId.BreedId;

        var isBreedExist = speciesResult.Value.Breeds.Any(b => b.Id.Value == breedId);
        if (!isBreedExist)
        {
            _logger.LogInformation("Breed with id = {breedId} was not found", breedId);
            _logger.LogWarning("Pet creation failed");
            return (ErrorList)Errors.General.NotFound("breed");
        }

        var breedAndSpeciesId = BreedAndSpeciesId.Create(speciesId, breedId).Value;

        var pet = new Domain.VolunteerAggregate.Entities.Pet(
            name,
            description,
            appearanceDetails,
            healthDetails,
            address,
            phoneNumber,
            command.Birthday,
            command.Status,
            new PetPhotosShell([]),
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