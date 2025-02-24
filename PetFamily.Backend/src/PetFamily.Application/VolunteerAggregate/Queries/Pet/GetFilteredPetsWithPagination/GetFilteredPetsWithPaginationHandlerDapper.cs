using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Pet;
using PetFamily.Application.DTOs.Read;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Application.VolunteerAggregate.Queries.Pet.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationHandlerDapper :
    IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IValidator<GetFilteredPetsWithPaginationQuery> _validator;
    private readonly ILogger<GetFilteredPetsWithPaginationHandlerDapper> _logger;

    public GetFilteredPetsWithPaginationHandlerDapper(
        ISqlConnectionFactory sqlConnectionFactory,
        IValidator<GetFilteredPetsWithPaginationQuery> validator,
        ILogger<GetFilteredPetsWithPaginationHandlerDapper> logger)
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

        try
        {
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
                                                   phone_number,
                                                   birth_date,
                                                   status,
                                                   health_information,
                                                   is_castrated,
                                                   is_vaccinated,
                                                   position,
                                                   species_id,
                                                   breed_id,
                                                   volunteer_id,
                                                   requisites,
                                                   pet_photos
                                                   FROM pets
                                                   /**where**/
                                                   /**orderby**/
                                                   LIMIT @PageSize OFFSET @OffSet;
                                                   """);

            builder.Where("is_deleted = 'false'");

            if (!string.IsNullOrWhiteSpace(query.Name))
                builder.Where("name ILIKE @Name");

            if (query.Coloration.HasValue)
                builder.Where("coloration = @Coloration");

            if (query.Weight.HasValue)
                builder.Where("weight = @Weight");

            if (query.Height.HasValue)
                builder.Where("height = @Height");

            if (!string.IsNullOrWhiteSpace(query.Country))
                builder.Where("country ILIKE @Country");

            if (!string.IsNullOrWhiteSpace(query.City))
                builder.Where("city ILIKE @City");

            if (!string.IsNullOrWhiteSpace(query.Street))
                builder.Where("street ILIKE @Street");

            if (!string.IsNullOrWhiteSpace(query.PostalCode))
                builder.Where("postal_code ILIKE @PostalCode");

            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
                builder.Where("phone_number ILIKE @PhoneNumber");

            if (query.BirthDate.HasValue)
                builder.Where("birth_date = @BirthDate");

            if (query.Status.HasValue)
                builder.Where("status = @Status");

            if (query.IsCastrated.HasValue)
                builder.Where("is_castrated = @IsCastrated");

            if (query.IsVaccinated.HasValue)
                builder.Where("is_vaccinated = @IsVaccinated");

            if (query.VolunteerId.HasValue)
                builder.Where("volunteer_id = @VolunteerId");

            if (query.BreedId.HasValue)
                builder.Where("breed_id = @BreedId");

            if (query.SpeciesId.HasValue)
                builder.Where("species_id = @SpeciesId");

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
                PhoneNumber = query.PhoneNumber + '%',
                BirthDate = query.BirthDate,
                Status = query.Status,
                IsCastrated = query.IsCastrated,
                IsVaccinated = query.IsVaccinated,
                SpeciesId = query.SpeciesId,
                BreedId = query.BreedId,
                VolunteerId = query.VolunteerId,
                SortDirection = query.SortDirection,
                SortBy = query.SortBy
            };

            var totalCount = await connection.ExecuteScalarAsync<long>(countTemplate.RawSql, param);

            var pets = await connection.QueryAsync<PetDto, string, string, PetDto>(
                petsTemplate.RawSql,
                (pet, jsonRequisites, jsonPetPhotos) =>
                {
                    pet.Requisites = JsonSerializer.Deserialize<RequisiteDto[]>(jsonRequisites)!;
                    pet.PetPhotos = JsonSerializer.Deserialize<PetPhotoDto[]>(jsonPetPhotos)!;
                    
                    return pet;
                },
                splitOn: "requisites,pet_photos",
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
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get pets with pagination");
            return (ErrorList)Errors.Database.IsFailure();
        }
    }
}