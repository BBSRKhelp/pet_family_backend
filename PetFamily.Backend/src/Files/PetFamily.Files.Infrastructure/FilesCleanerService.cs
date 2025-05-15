using Microsoft.Extensions.Logging;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Files.Application;

namespace PetFamily.Files.Infrastructure;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileIdentifier>> _messageQueue;
    private readonly ILogger<FilesCleanerService> _logger;


    public FilesCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileIdentifier>> messageQueue,
        ILogger<FilesCleanerService> logger)
    {
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Search for files to clean up");
        var fileIdentifiers = await _messageQueue.ReadAsync(cancellationToken);

        _logger.LogInformation("Cleaning up files");
        foreach (var fileIdentifier in fileIdentifiers)
        {
            await _fileProvider.RemoveFileAsync(fileIdentifier, cancellationToken);
        }

        _logger.LogInformation("Files have been cleared");
    }
}