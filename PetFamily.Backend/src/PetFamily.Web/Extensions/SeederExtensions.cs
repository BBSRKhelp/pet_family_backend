using PetFamily.Accounts.Infrastructure.Seeding;

namespace PetFamily.Web.Extensions;

public static class SeederExtensions
{
    public static async Task UseAccountsSeederAsync(this IApplicationBuilder app)
    {
        var seeder = app.ApplicationServices.GetRequiredService<AccountsSeeder>();
        await seeder.SeedAsync();
    }
}