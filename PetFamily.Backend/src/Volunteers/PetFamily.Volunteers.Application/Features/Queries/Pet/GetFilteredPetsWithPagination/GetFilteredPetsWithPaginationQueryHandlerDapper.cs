using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryHandlerDapper :
    IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IValidator<GetFilteredPetsWithPaginationQuery> _validator;
    private readonly ILogger<GetFilteredPetsWithPaginationQueryHandlerDapper> _logger;

    public GetFilteredPetsWithPaginationQueryHandlerDapper(
        ISqlConnectionFactory sqlConnectionFactory,
        IValidator<GetFilteredPetsWithPaginationQuery> validator,
        ILogger<GetFilteredPetsWithPaginationQueryHandlerDapper> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> HandleAsync(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting pets with pagination");

        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Failed to get pets with pagination");
            return validationResult.ToErrorList();
        }

        using var connection = _sqlConnectionFactory.GetConnection();

        _logger.LogInformation("Connection with database established");

        var builder = new SqlBuilder();

        var countTemplate = builder.AddTemplate("SELECT COUNT(*) FROM pets /**where**/");

        var petsTemplate = builder.AddTemplate("""
                                               SELECT id,
                                               name,
                                               description,
                                               coloration,
                                               weight,
                                               height,
                                               country,
                                               city,
                                               street,
                                               postal_code,
                                               birth_date,
                                               status,
                                               health_information,
                                               is_castrated,
                                               is_vaccinated,
                                               position,
                                               species_id,
                                               breed_id,
                                               volunteer_id,
                                               pet_photos
                                               FROM pets
                                               /**where**/
                                               /**orderby**/
                                               LIMIT @PageSize OFFSET @OffSet;
                                               """);

        builder.Where("is_deleted = 'false'")
            .AddCondition("name ILIKE @Name", query.Name)
            .AddCondition("coloration = @Coloration", query.Coloration)
            .AddCondition("weight = @Weight", query.Weight)
            .AddCondition("height = @Height", query.Height)
            .AddCondition("country ILIKE @Country", query.Country)
            .AddCondition("city ILIKE @City", query.City)
            .AddCondition("street ILIKE @Street", query.Street)
            .AddCondition("postal_code ILIKE @PostalCode", query.PostalCode)
            .AddCondition("birth_date = @BirthDate", query.BirthDate)
            .AddCondition("status = @Status", query.Status)
            .AddCondition("is_castrated = @IsCastrated", query.IsCastrated)
            .AddCondition("is_vaccinated = @IsVaccinated", query.IsVaccinated)
            .AddCondition("position = @Position", query.Position)
            .AddCondition("volunteer_id = @VolunteerId", query.VolunteerId)
            .AddCondition("breed_id = @BreedId", query.BreedId)
            .AddCondition("species_id = @SpeciesId", query.SpeciesId);

        builder.OrderBy($"{query.SortBy} {query.SortDirection}");

        var param = new
        {
            PageSize = query.PageSize,
            OffSet = (query.PageNumber - 1) * query.PageSize,
            Name = '%' + query.Name + '%',
            Coloration = query.Coloration,
            Weight = query.Weight,
            Height = query.Height,
            Country = '%' + query.Country + '%',
            City = '%' + query.City + '%',
            Street = '%' + query.Street + '%',
            PostalCode = '%' + query.PostalCode + '%',
            BirthDate = query.BirthDate,
            Status = query.Status,
            IsCastrated = query.IsCastrated,
            IsVaccinated = query.IsVaccinated,
            Position = query.Position,
            SpeciesId = query.SpeciesId,
            BreedId = query.BreedId,
            VolunteerId = query.VolunteerId,
            SortDirection = query.SortDirection,
            SortBy = query.SortBy
        };

        var totalCount = await connection.ExecuteScalarAsync<long>(countTemplate.RawSql, param);

        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            petsTemplate.RawSql,
            (pet, jsonPetPhotos) =>
            {
                pet.PetPhotos = JsonSerializer.Deserialize<PetPhotoDto[]>(jsonPetPhotos)!;

                return pet;
            },
            splitOn: "pet_photos",
            param: param);

        _logger.LogInformation("Pets have been received");

        return new PagedList<PetDto>
        {
            Items = pets.ToList(),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}