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

public class GetFilteredVolunteersWithPaginationHandlerDapper :
    IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;
    private readonly ILogger<GetFilteredVolunteersWithPaginationHandlerDapper> _logger;

    public GetFilteredVolunteersWithPaginationHandlerDapper(
        ISqlConnectionFactory sqlConnectionFactory,
        IValidator<GetFilteredVolunteersWithPaginationQuery> validator,
        ILogger<GetFilteredVolunteersWithPaginationHandlerDapper> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> HandleAsync(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting volunteers with pagination");

        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Failed to get volunteers with pagination");
            return validationResult.ToErrorList();
        }

        using var connection = _sqlConnectionFactory.GetConnection();

        _logger.LogInformation("Connection with database established");

        var builder = new SqlBuilder();

        var countTemplate = builder.AddTemplate("SELECT COUNT(*) FROM volunteers /**where**/");

        var volunteersTemplate = builder.AddTemplate("""
                                                     SELECT id,
                                                     first_name, 
                                                     last_name, 
                                                     patronymic, 
                                                     description, 
                                                     work_experience,
                                                     phone_number,
                                                     email,
                                                     FROM volunteers
                                                     /**where**/ 
                                                     /**orderby**/
                                                     LIMIT @PageSize OFFSET @OffSet;
                                                     """);

        builder.Where("is_deleted = 'false'");

        if (!string.IsNullOrWhiteSpace(query.FirstName))
            builder.Where("first_name ILIKE @FirstName");

        if (!string.IsNullOrWhiteSpace(query.LastName))
            builder.Where("last_name ILIKE @LastName");

        if (!string.IsNullOrWhiteSpace(query.Patronymic))
            builder.Where("patronymic ILIKE @Patronymic");

        if (query.WorkExperience.HasValue)
            builder.Where("work_experience = @WorkExperience");

        builder.OrderBy($"{query.SortBy} {query.SortDirection}");

        var param = new
        {
            PageSize = query.PageSize,
            OffSet = (query.PageNumber - 1) * query.PageSize,
            FirstName = '%' + query.FirstName + '%',
            LastName = '%' + query.LastName + '%',
            Patronymic = '%' + query.Patronymic + '%',
            WorkExperience = query.WorkExperience,
            SortDirection = query.SortDirection,
            SortBy = query.SortBy
        };

        var totalCount = await connection.ExecuteScalarAsync<long>(countTemplate.RawSql, param);

        var volunteers = await connection.QueryAsync<VolunteerDto>(
            volunteersTemplate.RawSql,
            param: param);

        _logger.LogInformation("Volunteers have been received");

        return new PagedList<VolunteerDto>
        {
            Items = volunteers.ToList(),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}