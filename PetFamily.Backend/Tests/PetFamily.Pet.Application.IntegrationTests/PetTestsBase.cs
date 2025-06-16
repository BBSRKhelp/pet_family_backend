using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Enums;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Domain.Entities;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects;
using VolunteerWriteDbContext = PetFamily.Volunteers.Infrastructure.Database.WriteDbContext;
using SpeciesWriteDbContext = PetFamily.Species.Infrastructure.Database.WriteDbContext;

namespace PetFamily.Pet.Application.IntegrationTests;

public class PetTestsBase : IClassFixture<PetTestsWebFactory>, IAsyncLifetime
{
    protected readonly PetTestsWebFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly Fixture Fixture;
    protected readonly VolunteerWriteDbContext VolunteerWriteDbContext;
    protected readonly SpeciesWriteDbContext SpeciesWriteDbContext;

    protected PetTestsBase(PetTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        Fixture = new Fixture();
        VolunteerWriteDbContext = Scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        SpeciesWriteDbContext = Scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
    }

    protected async Task<Volunteers.Domain.Entities.Pet> SeedPetAsync(
        Volunteer volunteer,
        SpeciesId speciesId,
        Guid breedId)
    {
        var pet = new Volunteers.Domain.Entities.Pet(
            Name.Create("testname").Value,
            Description.Create("").Value,
            AppearanceDetails.Create(Colour.Black, 15, 15).Value,
            HealthDetails.Create("string.Empty", true, true).Value,
            Address.Create("string.Empty", "string.Empty", "string.Empty", "string.Empty").Value,
            PhoneNumber.Create("89166666666").Value,
            DateOnly.Parse("2023-12-23"),
            Status.FoundHome,
            [],
            BreedAndSpeciesId.Create(speciesId, breedId).Value);
        
        volunteer.AddPet(pet);
        await VolunteerWriteDbContext.SaveChangesAsync();
        
        return pet;
    }
    
    protected async Task<Volunteer> SeedVolunteerAsync()
    {
        var volunteer = new Volunteer(
            FullName.Create("testname", "testlastname", "testpatronymic").Value,
            Email.Create("test@test.com").Value,
            Description.Create("").Value,
            WorkExperience.Create(46).Value,
            PhoneNumber.Create("89166666666").Value);
        
        await VolunteerWriteDbContext.Volunteers.AddAsync(volunteer);
        await VolunteerWriteDbContext.SaveChangesAsync();
        
        return volunteer;
    }

    protected async Task<PetFamily.Species.Domain.Species> SeedSpeciesAsync()
    {
        var species = new Species.Domain.Species(Name.Create("testname").Value);
        
        await SpeciesWriteDbContext.Species.AddAsync(species);
        await SpeciesWriteDbContext.SaveChangesAsync();
        
        return species;
    }

    protected async Task<Guid> SeedBreedAsync(PetFamily.Species.Domain.Species species)
    {
        var breed = new Breed(Name.Create("testname").Value);
        
        species.AddBreed(breed);
        await SpeciesWriteDbContext.SaveChangesAsync();
        
        return breed.Id.Value;
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}