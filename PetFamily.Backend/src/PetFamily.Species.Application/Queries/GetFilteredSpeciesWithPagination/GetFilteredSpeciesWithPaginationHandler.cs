using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Extensions;
using PetFamily.Core.Interfaces.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Species.Application.DTOs.Read;
using PetFamily.Species.Application.Interfaces;

namespace PetFamily.Species.Application.Queries.GetFilteredSpeciesWithPagination;

public class GetFilteredSpeciesWithPaginationHandler :
    IQueryHandler<PagedList<SpeciesDto>, GetFilteredSpeciesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetFilteredSpeciesWithPaginationQuery> _validator;
    private readonly ILogger<GetFilteredSpeciesWithPaginationHandler> _logger;

    public GetFilteredSpeciesWithPaginationHandler(
        IReadDbContext readDbContext,
        IValidator<GetFilteredSpeciesWithPaginationQuery> validator,
        ILogger<GetFilteredSpeciesWithPaginationHandler> logger)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> HandleAsync(
        GetFilteredSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting species with pagination");

        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogInformation("Failed to get species with pagination");
            return validationResult.ToErrorList();
        }

        _logger.LogInformation("Species have been received");
        
        return await _readDbContext.Species
            .WhereIf(
                !string.IsNullOrWhiteSpace(query.Name),
                s => EF.Functions.ILike(s.Name, $"%{query.Name}%"))
            .OrderByDynamic(query.SortBy, query.SortDirection)
            .ToPagedList(query.PageNumber, query.PageSize, cancellationToken);
    }
}