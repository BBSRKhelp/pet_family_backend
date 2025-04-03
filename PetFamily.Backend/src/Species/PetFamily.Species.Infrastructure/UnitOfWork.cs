using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Infrastructure.Database;

namespace PetFamily.Species.Infrastructure;

public class UnitOfWork(WriteDbContext dbContext) : IUnitOfWork
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