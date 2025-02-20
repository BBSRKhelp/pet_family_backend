using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.IntegrationTests.Breed;

public class BreedTestsBase : IClassFixture<BreedTestsWebFactory>, IAsyncLifetime
{
    private readonly BreedTestsWebFactory _factory;
    protected readonly IServiceScope Scope;
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteDbContext;

    protected BreedTestsBase(BreedTestsWebFactory factory)
    {
        _factory = factory;
        Scope = factory.Services.CreateScope();
        Fixture = new Fixture();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    }

    protected async Task<Domain.SpeciesAggregate.Species> SeedSpeciesAsync()
    {
        var species = new Domain.SpeciesAggregate.Species(Name.Create("nameSpecies").Value);

        WriteDbContext.Species.Add(species);
        await WriteDbContext.SaveChangesAsync();

        return species;
    }

    protected async Task<Guid> SeedBreedAsync(Domain.SpeciesAggregate.Species species, string name)
    {
        var breed = new Domain.SpeciesAggregate.Entities.Breed(Name.Create(name).Value);

        species.AddBreed(breed);
        await WriteDbContext.SaveChangesAsync();

        return breed.Id.Value;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}