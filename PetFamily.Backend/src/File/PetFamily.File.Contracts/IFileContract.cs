using CSharpFunctionalExtensions;
using PetFamily.Core.Providers;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.File.Contracts;

public interface IFileContract
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default);
}