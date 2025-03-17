using CSharpFunctionalExtensions;
using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Interfaces.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> HandleAsync(TCommand command, 
        CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> HandleAsync(TCommand command, 
        CancellationToken cancellationToken = default);
}