using PetFamily.Accounts.Application.Interfaces.Managers;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Database;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers;

public class ParticipantAccountManager(AccountsDbContext accountsDbContext) : IParticipantAccountManager
{
    public async Task CreateParticipantAccountAsync(
        ParticipantAccount participantAccount,
        CancellationToken cancellationToken = default)
    {
        await accountsDbContext.ParticipantAccounts.AddAsync(participantAccount, cancellationToken);
        await accountsDbContext.SaveChangesAsync(cancellationToken);
    }
}