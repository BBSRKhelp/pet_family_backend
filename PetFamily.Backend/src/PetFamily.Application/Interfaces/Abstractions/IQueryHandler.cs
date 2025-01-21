using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.Interfaces.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse, ErrorList>> HandleAsync(TQuery query, 
        CancellationToken cancellationToken = default);
}