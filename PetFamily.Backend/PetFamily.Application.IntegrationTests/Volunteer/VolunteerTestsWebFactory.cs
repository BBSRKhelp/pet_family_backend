using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Infrastructure;

namespace PetFamily.Application.IntegrationTests.Volunteer;

public class VolunteerTestsWebFactory : IntegrationTestsWebFactory
{
    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);
                
        var sqlConnectionFactory = services.SingleOrDefault(s =>
            s.ServiceType == typeof(SqlConnectionFactory));
        
        if (sqlConnectionFactory is not null)
            services.Remove(sqlConnectionFactory);

        services.AddTransient<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(DbContainer.GetConnectionString()));
    }
}