using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteer.Contracts;

namespace PetFamily.Volunteer.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        return services.AddScoped<IVolunteerContract, VolunteerContract>();
    }
}