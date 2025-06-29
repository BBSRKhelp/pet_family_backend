using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Contracts.DTOs;

namespace PetFamily.Species.Application.Features.Queries.GetBreedsByIdSpecies;

public class GetBreedsByIdSpeciesHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsByIdSpeciesQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetBreedsByIdSpeciesQuery> _validator;
    private readonly ILogger<GetBreedsByIdSpeciesHandler> _logger;

    public GetBreedsByIdSpeciesHandler(
        IReadDbContext readDbContext,
        IValidator<GetBreedsByIdSpeciesQuery> validator,
        ILogger<GetBreedsByIdSpeciesHandler> logger)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> HandleAsync(
        GetBreedsByIdSpeciesQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting breeds by id species");

        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Failed to get breed by id");
            return validationResult.ToErrorList();
        }

        var breeds = await _readDbContext
            .Breeds
            .Where(b => b.SpeciesId == query.SpeciesId)
            .WhereIf(
                !string.IsNullOrWhiteSpace(query.Name),
                b => EF.Functions.ILike(b.Name, $"%{query.Name}%"))
            .OrderByDynamic(query.SortBy, query.SortDirection)
            .ToPagedList(query.PageNumber, query.PageSize, cancellationToken);

        return breeds;
    }
}