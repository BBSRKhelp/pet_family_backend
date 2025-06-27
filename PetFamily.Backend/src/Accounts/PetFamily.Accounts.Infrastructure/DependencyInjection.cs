using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Authorization.Managers;
using PetFamily.Accounts.Infrastructure.Authorization.Seeding;
using PetFamily.Accounts.Infrastructure.Database;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddProviders()
            .RegisterIdentity()
            .AddConfigurations(configuration)
            .AddDatabase(configuration)
            .AddManagers()
            .AddSeeding();
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddTransient<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        services.Configure<RefreshOptions>(configuration.GetSection(RefreshOptions.REFRESH));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AccountsDbContext>(_ =>
            new AccountsDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkContext.Accounts);

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(configuration.GetConnectionString(Constants.DATABASE)!));

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<AdminAccountManager>();
        services.AddScoped<IParticipantAccountManager,ParticipantAccountManager>();
        services.AddScoped<VolunteerAccountManager>();
        services.AddScoped<PermissionManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        
        return services;
    }

    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        
        return services;
    }
}