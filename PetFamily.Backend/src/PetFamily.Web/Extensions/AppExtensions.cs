using Microsoft.EntityFrameworkCore;
using SpeciesWriteDbContext = PetFamily.Species.Infrastructure.Database.WriteDbContext;
using VolunteerWriteDbContext = PetFamily.Volunteers.Infrastructure.Database.WriteDbContext;
using AccountsDbContext = PetFamily.Accounts.Infrastructure.Database.AccountsDbContext;

namespace PetFamily.Web.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var volunteerDbContext = scope.ServiceProvider.GetRequiredService<VolunteerWriteDbContext>();
        var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        var accountsDbContext = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

        await volunteerDbContext.Database.MigrateAsync();
        await speciesDbContext.Database.MigrateAsync();
        await accountsDbContext.Database.MigrateAsync();
    }
}