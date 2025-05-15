using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        return services.AddScoped<IVolunteerContract, VolunteerContract>();
    }
}