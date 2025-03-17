using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Files;
using PetFamily.Application.Interfaces.Messaging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Providers;
using PetFamily.Core.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.UploadFilesToPet;

public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileIdentifier>> _messageQueue;
    private readonly ILogger<UploadFilesToPetHandler> _logger;
    private const string BUCKET_NAME = "photos";

    public UploadFilesToPetHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IValidator<UploadFilesToPetCommand> validator,
        IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileIdentifier>> messageQueue,
        ILogger<UploadFilesToPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
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
            {
                _logger.LogWarning("Upload files to pet failed");
                return validationResult.ToErrorList();
            }

            var volunteerResult = await _volunteersRepository.GetByIdAsync(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
            {
                _logger.LogWarning("Upload files to pet failed");
                return (ErrorList)volunteerResult.Error;
            }

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var photoPath = PhotoPath.Create(file.FileName).Value;

                var fileData = new FileData(file.Stream, new FileIdentifier(photoPath, BUCKET_NAME));

                filesData.Add(fileData);
            }

            var filePathsResult = await _fileProvider.UploadFilesAsync(filesData, cancellationToken);
            if (filePathsResult.IsFailure)
            {
                _logger.LogError("Upload files to pet failed");
                _logger.LogInformation("Writing files for cleaning");
                await _messageQueue.WriteAsync(filesData.Select(f => f.FileIdentifier), cancellationToken);

                return (ErrorList)filePathsResult.Error;
            }

            var petPhotos = filePathsResult.Value
                    .Select(p => p)
                    .Select(p => new PetPhoto(p)).ToArray();

            var petResult = volunteerResult.Value.GetPetById(command.PetId);
            if (petResult.IsFailure)
            {
                _logger.LogError("Upload files to pet failed");
                return (ErrorList)petResult.Error;
            }

            petResult.Value.AddPhotos(petPhotos);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Success uploaded files to pet with Id = {PetId}", petResult.Value.Id.Value);

            transaction.Commit();

            return petResult.Value.Id.Value;
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