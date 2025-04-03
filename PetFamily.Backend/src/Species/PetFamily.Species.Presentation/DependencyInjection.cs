using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts;
using PetFamily.Species.Infrastructure;

namespace PetFamily.Species.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSpecies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddApplication()
            .AddInfrastructure(configuration)
            .AddScoped<ISpeciesContract, SpeciesContract>();
    }
}