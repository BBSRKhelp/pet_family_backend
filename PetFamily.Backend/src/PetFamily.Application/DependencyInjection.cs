using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.SpeciesAggregate.Commands.AddBreed;
using PetFamily.Application.SpeciesAggregate.Commands.Create;
using PetFamily.Application.VolunteerAggregate.Commands.AddPet;
using PetFamily.Application.VolunteerAggregate.Commands.ChangePetsPosition;
using PetFamily.Application.VolunteerAggregate.Commands.Create;
using PetFamily.Application.VolunteerAggregate.Commands.Delete;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateMainInfo;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateRequisites;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateSocialNetworks;
using PetFamily.Application.VolunteerAggregate.Commands.UploadFilesToPet;
using PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;
using PetFamily.Application.VolunteerAggregate.Queries.GetVolunteerById;

namespace PetFamily.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(c => c.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}