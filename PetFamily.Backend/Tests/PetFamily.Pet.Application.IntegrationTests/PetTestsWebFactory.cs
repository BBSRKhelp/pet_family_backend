using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetFamily.Core.Providers;
using PetFamily.Files.Application;
using PetFamily.Shared.Application.IntegrationTests;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Pet.Application.IntegrationTests;

public class PetTestsWebFactory : IntegrationTestsWebFactory
{
    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);

        var fileProvider = services.SingleOrDefault(s => s.ServiceType == typeof(IFileProvider));
        if (fileProvider is not null)
            services.Remove(fileProvider);

        services.AddTransient<IFileProvider>(_ => _fileProviderMock);
    }

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