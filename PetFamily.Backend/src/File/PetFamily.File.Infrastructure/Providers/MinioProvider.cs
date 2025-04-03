using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.Providers;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using IFileProvider = PetFamily.File.Application.IFileProvider;

namespace PetFamily.File.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 5;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFilesAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketNotExistCreateBucketAsync(filesList, cancellationToken);

            var tasks = filesList
                .Select(async file => await PutObjectAsync(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);
            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var result = pathsResult.Select(p => p.Value).ToList();

            _logger.LogInformation("{count} files were uploaded in MinIO", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload files in minio, files amount: {amount}", filesList.Count);
            return Error.Failure("file.not.upload", "fail to upload files in MinIO");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    public async Task<UnitResult<Error>> RemoveFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting file");

            var fileExist = await FileIsExisted(fileIdentifier, cancellationToken);

            if (fileExist)
            {
                _logger.LogInformation("File with path '{filePath}' in bucket '{bucketName} does not exist",
                    fileIdentifier.PhotoPath.Path, fileIdentifier.BucketName);
                return Result.Success<Error>();
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileIdentifier.BucketName)
                .WithObject(fileIdentifier.PhotoPath.Path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            _logger.LogInformation("Deleted a file '{filePath}' from '{bucketName}' in MinIO",
                fileIdentifier.PhotoPath.Path, fileIdentifier.BucketName);

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file from MinIO");
            return Error.Failure("file.delete", "fail to delete file in MinIO");
        }
    }

    public async Task<Result<string, Error>> GetFileAsync(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting file");

            var fileExist = await FileIsExisted(fileIdentifier, cancellationToken);

            if (!fileExist)
            {
                _logger.LogInformation("File with path '{filePath}' in bucket '{bucketName} does not exist",
                    fileIdentifier.BucketName, fileIdentifier.PhotoPath.Path);
                return Error.NotFound("file.not.found", $"file '{fileIdentifier.PhotoPath.Path}' not found in MinIO");
            }

            var getFileArgs = new PresignedGetObjectArgs()
                .WithBucket(fileIdentifier.BucketName)
                .WithObject(fileIdentifier.PhotoPath.Path)
                .WithExpiry(86400);

            return await _minioClient.PresignedGetObjectAsync(getFileArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get file from MinIO");
            return Error.Failure("file.not.get", "fail to get file in MinIO");
        }
    }

    private async Task<Result<PhotoPath, Error>> PutObjectAsync(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileIdentifier.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FileIdentifier.PhotoPath.Path);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return fileData.FileIdentifier.PhotoPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file in MinIO with path '{filePath}' in bucket '{bucketName}'",
                fileData.FileIdentifier.PhotoPath.Path, fileData.FileIdentifier.BucketName);
            return Error.Failure("file.upload", "Fail to upload file in MinIO");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketNotExistCreateBucketAsync(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.FileIdentifier.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!bucketExist)
            {
                _logger.LogInformation("Bucket '{bucketName}' does not exist", bucketName);

                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);

                _logger.LogInformation("Created bucket '{bucketName}'", bucketName);
            }
        }
    }

    private async Task<bool> FileIsExisted(
        FileIdentifier fileIdentifier,
        CancellationToken cancellationToken = default)
    {
        var statObjectArgs = new StatObjectArgs()
            .WithBucket(fileIdentifier.BucketName)
            .WithObject(fileIdentifier.PhotoPath.Path);
        var fileExist = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

        return string.IsNullOrWhiteSpace(fileExist.ContentType);
    }
}