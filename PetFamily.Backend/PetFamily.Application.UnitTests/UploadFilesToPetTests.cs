using System.Data;
using System.Data.Common;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pet;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Interfaces.Messaging;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteerAggregate.Commands.UploadFilesToPet;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;
using IFileProvider = PetFamily.Application.Interfaces.Files.IFileProvider;

namespace PetFamily.Application.UnitTests;

public class UploadFilesToPetTests
{
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock = new();
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IValidator<UploadFilesToPetCommand>> _validatorMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<FileIdentifier>>> _messageQueueMock = new();
    private readonly Mock<ILogger<UploadFilesToPetHandler>> _loggerMock = new();

    [Fact]
    public async Task Handle_ShouldUploadFilesToPet()
    {
        //Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        var volunteer = Shared.Models.CreateVolunteer();
        var pet = Shared.Models.CreatePet();
        volunteer.AddPet(pet);

        var stream = new MemoryStream();
        const string FILENAME = "test.jpg";
        var uploadFileDto = new UploadFileDto(stream, FILENAME);
        List<UploadFileDto> files = [uploadFileDto, uploadFileDto];

        var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, files);

        //volunteersRepository
        _volunteersRepositoryMock
            .Setup(r => r.GetByIdAsync(volunteer.Id, cancellationToken))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        //fileProvider
        List<PhotoPath> photoPaths =
        [
            PhotoPath.Create("jpg").Value,
            PhotoPath.Create("jpg").Value
        ];

        _fileProviderMock
            .Setup(p => p.UploadFilesAsync(It.IsAny<IEnumerable<FileData>>(), cancellationToken))
            .ReturnsAsync(Result.Success<IReadOnlyList<PhotoPath>, Error>(photoPaths));

        //validator
        _validatorMock
            .Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        //unitOfWork
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.BeginTransactionAsync(cancellationToken))
            .ReturnsAsync(new Mock<IDbTransaction>().Object);

        //messageQueue
        List<FileIdentifier> filesIdentifier =
        [
            new FileIdentifier(PhotoPath.Create("jpg").Value, "BucketName"),
            new FileIdentifier(PhotoPath.Create("jpg").Value, "BucketName")
        ];

        _messageQueueMock.Setup(m => m.WriteAsync(filesIdentifier, cancellationToken));

        //createHandler
        var handler = new UploadFilesToPetHandler(
            _volunteersRepositoryMock.Object,
            _fileProviderMock.Object,
            _validatorMock.Object,
            _unitOfWorkMock.Object,
            _messageQueueMock.Object,
            _loggerMock.Object);

        //Act
        var result = await handler.HandleAsync(command, cancellationToken);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Id.Value);
    }
}