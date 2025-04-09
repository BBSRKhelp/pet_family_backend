using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Infrastructure.Database;

namespace PetFamily.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddDatabase(configuration)
            .AddRepositories();
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkContext.Species);

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(configuration.GetConnectionString(Constants.DATABASE)!));

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }
}