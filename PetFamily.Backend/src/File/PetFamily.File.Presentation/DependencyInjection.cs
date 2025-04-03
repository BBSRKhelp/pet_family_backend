using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.File.Contracts;
using PetFamily.File.Infrastructure;

namespace PetFamily.File.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddFile(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddInfrastructure(configuration)
            .AddScoped<IFileContract, FileContract>();
    }
}