using CSharpFunctionalExtensions;
using PetFamily.Core.Providers;
using PetFamily.File.Application;
using PetFamily.File.Contracts;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.File.Presentation;

public class FileContract(IFileProvider fileProvider) : IFileContract
{
    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFilesAsync(IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        return await fileProvider.UploadFilesAsync(filesData, cancellationToken);
    }

    public async Task<UnitResult<Error>> RemoveFileAsync(FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default)
    {
        return await fileProvider.RemoveFileAsync(fileIdentifier, cancellationToken);
    }

    public async Task<Result<string, Error>> GetFileAsync(FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default)
    {
        return await fileProvider.GetFileAsync(fileIdentifier, cancellationToken);
    }
}