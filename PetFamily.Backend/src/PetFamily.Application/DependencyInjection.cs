using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Commands.Volunteer.Delete;
using PetFamily.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;

namespace PetFamily.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<VolunteerCreateHandler>();
        services.AddScoped<VolunteerDeleteHandler>();
        services.AddScoped<VolunteerUpdateMainInfoHandler>();
        services.AddScoped<VolunteerUpdateRequisitesHandler>();
        services.AddScoped<VolunteerUpdateSocialNetworksHandler>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}