using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Enums;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteer.Domain.ValueObjects;
using PetFamily.Volunteer.Infrastructure.Database;
using VolunteerWriteDbContext = PetFamily.Volunteer.Infrastructure.Database.WriteDbContext;
using SpeciesWriteDbContext = PetFamily.Species.Infrastructure.Database.WriteDbContext;

namespace PetFamily.Application.IntegrationTests.Pet;

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

    protected async Task<PetFamily.Volunteer.Domain.Entities.Pet> SeedPetAsync(
        PetFamily.Volunteer.Domain.Volunteer volunteer,
        SpeciesId speciesId,
        Guid breedId)
    {
        var pet = new PetFamily.Volunteer.Domain.Entities.Pet(
            Name.Create("testname").Value,
            Description.Create("").Value,
            AppearanceDetails.Create(Colour.Black, 15, 15).Value,
            HealthDetails.Create("string.Empty", true, true).Value,
            Address.Create("string.Empty", "string.Empty", "string.Empty", "string.Empty").Value,
            PhoneNumber.Create("89166666666").Value,
            DateTime.Now,
            Status.FoundHome,
            [],
            BreedAndSpeciesId.Create(speciesId, breedId).Value);
        
        volunteer.AddPet(pet);
        await VolunteerWriteDbContext.SaveChangesAsync();
        
        return pet;
    }
    
    protected async Task<PetFamily.Volunteer.Domain.Volunteer> SeedVolunteerAsync()
    {
        var volunteer = new PetFamily.Volunteer.Domain.Volunteer(
            FullName.Create("testname", "testlastname", "testpatronymic").Value,
            Email.Create("test@test.com").Value,
            Description.Create("").Value,
            WorkExperience.Create(46).Value,
            PhoneNumber.Create("89166666666").Value,
            [],
            []);
        
        await VolunteerWriteDbContext.Volunteers.AddAsync(volunteer);
        await VolunteerWriteDbContext.SaveChangesAsync();
        
        return volunteer;
    }

    protected async Task<PetFamily.Species.Domain.Species> SeedSpeciesAsync()
    {
        var species = new PetFamily.Species.Domain.Species(Name.Create("testname").Value);
        
        await SpeciesWriteDbContext.Species.AddAsync(species);
        await SpeciesWriteDbContext.SaveChangesAsync();
        
        return species;
    }

    protected async Task<Guid> SeedBreedAsync(PetFamily.Species.Domain.Species species)
    {
        var breed = new PetFamily.Species.Domain.Entities.Breed(Name.Create("testname").Value);
        
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