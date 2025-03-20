using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DTOs.Pet;
using PetFamily.Core.Models;

namespace PetFamily.Application.Validation;

public class SpeciesAndBreedValidator
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<SpeciesAndBreedValidator> _logger;

    public SpeciesAndBreedValidator(
        IReadDbContext readDbContext,
        ILogger<SpeciesAndBreedValidator> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<Boolean, ErrorList>> IsExist(
        BreedAndSpeciesIdDto breedAndSpeciesId, 
        CancellationToken cancellationToken = default)
    {
        var species = await _readDbContext
            .Species
            .FirstOrDefaultAsync(s => s.Id == breedAndSpeciesId.SpeciesId.Value, cancellationToken);
        
        if (species is null)
        {
            _logger.LogWarning("Species with id = {SpeciesId} was not found", breedAndSpeciesId.SpeciesId.Value);
            return (ErrorList)Errors.General.NotFound(nameof(species));
        }
        
        var breed = await _readDbContext
            .Breeds
            .FirstOrDefaultAsync(b => b.Id == breedAndSpeciesId.BreedId, cancellationToken);

        if (breed is null)
        {
            _logger.LogWarning("Breed with id = {BreedId} was not found", breedAndSpeciesId.BreedId);
            return (ErrorList)Errors.General.NotFound(nameof(breed));
        }

        return true;
    }
}