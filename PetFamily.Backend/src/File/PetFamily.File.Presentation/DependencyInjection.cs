using Microsoft.Extensions.DependencyInjection;
using PetFamily.File.Contracts;

namespace PetFamily.File.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddFilePresentation(this IServiceCollection services)
    {
        return services.AddScoped<IFileContract, FileContract>();
    }
}