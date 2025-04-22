using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Files.Application;

namespace PetFamily.Files.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FilesCleanerBackgroundService> _logger;

    public FilesCleanerBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<FilesCleanerBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting.");
        await using var scope = _scopeFactory.CreateAsyncScope();

        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFilesCleanerService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            await filesCleanerService.Process(stoppingToken);
        }

        await Task.CompletedTask;
        _logger.LogInformation("FilesCleanerBackgroundService is stopping.");
    }
}