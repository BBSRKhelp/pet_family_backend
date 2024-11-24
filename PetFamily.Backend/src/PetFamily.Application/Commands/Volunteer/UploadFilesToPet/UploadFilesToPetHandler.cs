using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Providers;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using PetFamily.Domain.VolunteerAggregate.ValueObjects.Shell;

namespace PetFamily.Application.Commands.Volunteer.UploadFilesToPet;

public class UploadFilesToPetHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UploadFilesToPetHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public UploadFilesToPetHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IValidator<UploadFilesToPetCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<UploadFilesToPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        UploadFilesToPetCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Uploading files to pet");

        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToErrorList();

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Upload files to pet failed");
                return (ErrorList)volunteerResult.Error;
            }

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var photoPath = PhotoPath.Create(Path.GetExtension(file.FileName)).Value;

                var fileData = new FileData(file.Stream, photoPath, BUCKET_NAME);

                filesData.Add(fileData);
            }

            var petPhotos = new PetPhotosShell(
                filesData
                    .Select(f => f.PhotoPath)
                    .Select(p => new PetPhoto(p)));

            var pet = volunteerResult.Value.GetPetById(command.PetId);
            if (pet.IsFailure)
            {
                _logger.LogError("Upload files to pet failed");
                return (ErrorList)pet.Error;
            }

            pet.Value.UpdatePhotos(petPhotos);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Upload files to pet with Id = {PetId} success", pet.Value.Id.Value);

            var uploadResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
            if (uploadResult.IsFailure)
                return (ErrorList)uploadResult.Error;

            transaction.Commit();

            return pet.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Upload files to pet failed");

            transaction.Rollback();

            return (ErrorList)Error.Failure("upload.files.error",
                $"upload files to pet with Id = {command.PetId} failed");
        }
    }
}