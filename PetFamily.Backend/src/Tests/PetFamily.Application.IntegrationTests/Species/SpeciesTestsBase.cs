using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Infrastructure.Database;

namespace PetFamily.Application.IntegrationTests.Species;

public class SpeciesTestsBase : IClassFixture<SpeciesTestsWebFactory>, IAsyncLifetime
{
    private readonly SpeciesTestsWebFactory _factory;
    protected readonly IServiceScope Scope;
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteDbContext;
    
    protected SpeciesTestsBase(SpeciesTestsWebFactory factory)
    {
        _factory = factory;
        Scope = factory.Services.CreateScope();
        Fixture = new Fixture();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    }

    protected async Task<Guid> SeedSpeciesAsync()
    {
        var species = new PetFamily.Species.Domain.Species(Name.Create("nameSpecies").Value);
        
        WriteDbContext.Species.Add(species);
        await WriteDbContext.SaveChangesAsync();
        
        return species.Id.Value;
    } 
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}