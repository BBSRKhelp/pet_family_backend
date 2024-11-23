using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Commands.Species.AddBreed;
using PetFamily.Application.Commands.Species.Create;
using PetFamily.Application.Commands.Volunteer.AddPet;
using PetFamily.Application.Commands.Volunteer.Create;
using PetFamily.Application.Commands.Volunteer.Delete;
using PetFamily.Application.Commands.Volunteer.UpdateMainInfo;
using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Commands.Volunteer.UpdateSocialNetworks;
using PetFamily.Application.Commands.Volunteer.UploadFilesToPet;
using PetFamily.Application.Database;

namespace PetFamily.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<UpdateMainInfoVolunteerHandler>();
        services.AddScoped<UpdateRequisitesVolunteerHandler>();
        services.AddScoped<UpdateSocialNetworksVolunteerHandler>();
        services.AddScoped<UploadFilesToPetHandler>();
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<CreateBreedHandler>();
        services.AddScoped<CreatePetHandler>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}