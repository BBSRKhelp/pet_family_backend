using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Accounts.Infrastructure.Database;
using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Infrastructure;

internal class UnitOfWork(AccountsDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken); 
    }
}