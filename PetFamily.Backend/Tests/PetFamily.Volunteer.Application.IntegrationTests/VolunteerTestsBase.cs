using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Shared.Application.IntegrationTests;
using PetFamily.Volunteers.Infrastructure.Database;

namespace PetFamily.Volunteer.Application.IntegrationTests;

public class VolunteerTestsBase : IClassFixture<VolunteerTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    protected readonly IServiceScope Scope;
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteDbContext;

    protected VolunteerTestsBase(VolunteerTestsWebFactory factory)
    {
        _factory = factory;
        Scope = factory.Services.CreateScope();
        Fixture = new Fixture();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    }

    protected async Task<Guid> SeedVolunteerAsync()
    {
        var volunteer = new Volunteers.Domain.Volunteer();
        
        await WriteDbContext.Volunteers.AddAsync(volunteer);
        await WriteDbContext.SaveChangesAsync();
        
        return volunteer.Id.Value;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}