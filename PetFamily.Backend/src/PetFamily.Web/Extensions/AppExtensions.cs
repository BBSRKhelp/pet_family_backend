using Microsoft.EntityFrameworkCore;
using SpeciesWriteDbContext = PetFamily.Species.Infrastructure.Database.WriteDbContext;
using VolunteerWriteDbContext = PetFamily.Volunteers.Infrastructure.Database.WriteDbContext;

namespace PetFamily.Web.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();

        await volunteerDbContext.Database.MigrateAsync();
        await speciesDbContext.Database.MigrateAsync();
    }
}