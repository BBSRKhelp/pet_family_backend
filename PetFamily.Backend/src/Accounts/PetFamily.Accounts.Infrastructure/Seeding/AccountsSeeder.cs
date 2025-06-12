using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeeder(IServiceScopeFactory scopeFactory)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateScope();

        var services = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

        await services.SeedAsync(cancellationToken);
    }
}