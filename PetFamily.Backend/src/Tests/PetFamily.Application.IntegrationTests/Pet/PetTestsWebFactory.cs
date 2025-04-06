using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetFamily.Core.Providers;
using PetFamily.File.Application;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Application.IntegrationTests.Pet;

public class PetTestsWebFactory : IntegrationTestsWebFactory
{
    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    // private readonly IMessageQueue<IEnumerable<FileIdentifier>> _messageQueueMock =
    //     Substitute.For<IMessageQueue<IEnumerable<FileIdentifier>>>();

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);

        var fileProvider = services.SingleOrDefault(s => s.ServiceType == typeof(IFileProvider));
        if (fileProvider is not null)
            services.Remove(fileProvider);

        services.AddTransient<IFileProvider>(_ => _fileProviderMock);

        // Думал еще обернуть messageQueue в Mock, но не получилось, видимо с дэдлоком сталкивался
        
        // var messageQueue = services
        //     .SingleOrDefault(s => s.ServiceType == typeof(IMessageQueue<IEnumerable<FileIdentifier>>));
        //
        // if (messageQueue is not null)
        //     services.Remove(messageQueue);
        //
        // services.AddTransient<IMessageQueue<IEnumerable<FileIdentifier>>>(_ => _messageQueueMock);
    }

    // public void SetupMessageQueueMock()
    // {
    //     _messageQueueMock
    //         .WriteAsync(Arg.Any<IEnumerable<FileIdentifier>>(), Arg.Any<CancellationToken>())
    //         .ReturnsForAnyArgs(Task.CompletedTask);
    // }

    public void SetupSuccessFileProviderMock()
    {
        var photoPath = PhotoPath.Create("photo.jpg").Value;

        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>())
            .Returns(Task.FromResult(Result.Success<IReadOnlyList<PhotoPath>, Error>([photoPath])));
    }

    public void SetupFailureFileProviderMock()
    {
        _fileProviderMock
            .UploadFilesAsync(Arg.Any<IEnumerable<FileData>>())
            .Returns(Task.FromResult(Result.Failure<IReadOnlyList<PhotoPath>, Error>(Errors.General.IsInvalid())));
    }
}