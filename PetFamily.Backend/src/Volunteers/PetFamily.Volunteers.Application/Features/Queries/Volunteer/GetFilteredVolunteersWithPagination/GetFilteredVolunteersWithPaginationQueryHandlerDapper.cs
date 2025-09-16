using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetFilteredVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationQueryHandlerDapper(
    ISqlConnectionFactory sqlConnectionFactory,
    IValidator<GetFilteredVolunteersWithPaginationQuery> validator,
    ILogger<GetFilteredVolunteersWithPaginationQueryHandlerDapper> logger)
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> HandleAsync(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting volunteers with pagination");

        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Failed to get volunteers with pagination");
            return validationResult.ToErrorList();
        }

        using var connection = sqlConnectionFactory.GetConnection();

        logger.LogInformation("Connection with database established");

        var builder = new SqlBuilder();

        var countTemplate = builder.AddTemplate("SELECT COUNT(*) FROM volunteers /**where**/");

        var volunteersTemplate = builder.AddTemplate("""
                                                     SELECT id
                                                     FROM volunteers
                                                     /**where**/ 
                                                     /**orderby**/
                                                     LIMIT @PageSize OFFSET @OffSet;
                                                     """);

        builder.Where("is_deleted = 'false'");

        builder.OrderBy($"{query.SortBy} {query.SortDirection}");

        var param = new
        {
            PageSize = query.PageSize,
            OffSet = (query.PageNumber - 1) * query.PageSize,
            SortDirection = query.SortDirection,
            SortBy = query.SortBy
        };

        var totalCount = await connection.ExecuteScalarAsync<long>(countTemplate.RawSql, param);

        var volunteers = await connection.QueryAsync<VolunteerDto>(
            volunteersTemplate.RawSql,
            param: param);

        logger.LogInformation("Volunteers have been received");

        return new PagedList<VolunteerDto>
        {
            Items = volunteers.ToList(),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}