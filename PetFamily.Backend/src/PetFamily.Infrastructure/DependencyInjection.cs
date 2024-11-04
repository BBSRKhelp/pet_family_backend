using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        return services;
    }
}