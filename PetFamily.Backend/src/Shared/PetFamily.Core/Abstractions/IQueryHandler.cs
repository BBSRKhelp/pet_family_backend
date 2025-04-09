using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, 
        CancellationToken cancellationToken = default);
}