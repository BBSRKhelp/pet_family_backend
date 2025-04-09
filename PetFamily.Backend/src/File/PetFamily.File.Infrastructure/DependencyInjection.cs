using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.Messaging;
using PetFamily.Core.Options;
using PetFamily.Core.Providers;
using PetFamily.File.Application;
using PetFamily.File.Infrastructure.BackgroundServices;
using PetFamily.File.Infrastructure.MessageQueues;
using PetFamily.File.Infrastructure.Providers;

namespace PetFamily.File.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFileInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        return services.AddBackgroundService()
            .AddFileProvider(configuration);
    }
    
    private static IServiceCollection AddBackgroundService(
        this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddScoped<IFilesCleanerService, FilesCleanerService>();
        services.AddSingleton<IMessageQueue<IEnumerable<FileIdentifier>>,
            InMemoryMessageQueue<IEnumerable<FileIdentifier>>>();

        return services;
    }

    private static IServiceCollection AddFileProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}