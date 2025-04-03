using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteer.Application;
using PetFamily.Volunteer.Contracts;
using PetFamily.Volunteer.Infrastructure;

namespace PetFamily.Volunteer.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteer(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddApplication()
            .AddInfrastructure(configuration)
            .AddScoped<IVolunteerContract, VolunteerContract>();
    }
}