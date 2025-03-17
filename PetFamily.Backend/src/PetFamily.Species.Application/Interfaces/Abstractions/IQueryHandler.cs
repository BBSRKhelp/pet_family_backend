using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Interfaces.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, 
        CancellationToken cancellationToken = default);
}