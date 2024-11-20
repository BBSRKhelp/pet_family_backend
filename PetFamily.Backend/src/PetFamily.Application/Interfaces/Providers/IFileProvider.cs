using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.Models;
using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Interfaces.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> DeleteFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default);
}