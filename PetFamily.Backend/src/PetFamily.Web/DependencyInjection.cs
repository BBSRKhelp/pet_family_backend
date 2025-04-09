using PetFamily.File.Infrastructure;
using PetFamily.File.Presentation;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.Volunteer.Application;
using PetFamily.Volunteer.Infrastructure;
using PetFamily.Volunteer.Presentation;
using Serilog;
using Serilog.Events;

namespace PetFamily.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSerilog(configuration);

        services.AddFilePresentation()
            .AddFileInfrastructure(configuration)
            .AddSpeciesPresentation()
            .AddSpeciesApplication()
            .AddSpeciesInfrastructure(configuration)
            .AddVolunteerPresentation()
            .AddVolunteerApplication()
            .AddVolunteerInfrastructure(configuration);
        
        return services;
    }

    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .Enrich.WithThreadId()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }
}