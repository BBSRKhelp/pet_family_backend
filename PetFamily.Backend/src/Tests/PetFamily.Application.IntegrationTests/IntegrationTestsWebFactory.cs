using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.Application.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer DbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));

        var readContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IReadDbContext));

        if (writeContext is not null)
            services.Remove(writeContext);

        if (readContext is not null)
            services.Remove(readContext);

        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(DbContainer.GetConnectionString()));

        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(DbContainer.GetConnectionString()));
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = []
            }
        );
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(DbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await DbContainer.StopAsync();
        await DbContainer.DisposeAsync();
    }
}