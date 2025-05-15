using Microsoft.Extensions.DependencyInjection;
using PetFamily.Files.Contracts;

namespace PetFamily.Files.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddFilePresentation(this IServiceCollection services)
    {
        return services.AddScoped<IFileContract, FileContract>();
    }
}