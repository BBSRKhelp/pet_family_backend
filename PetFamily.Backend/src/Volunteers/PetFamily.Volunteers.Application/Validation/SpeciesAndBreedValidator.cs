using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Application.Validation;

public class SpeciesAndBreedValidator
{
    private readonly ISpeciesContract _speciesContract;
    private readonly ILogger<SpeciesAndBreedValidator> _logger;

    public SpeciesAndBreedValidator(
        ISpeciesContract speciesContract,
        ILogger<SpeciesAndBreedValidator> logger)
    {
        _speciesContract = speciesContract;
        _logger = logger;
    }

    public async Task<Result<Boolean, ErrorList>> IsExist(
        BreedAndSpeciesIdDto breedAndSpeciesId,
        CancellationToken cancellationToken = default)
    {
        var speciesIsExist = await _speciesContract.SpeciesExistsAsync(
            breedAndSpeciesId.SpeciesId.Value, 
            cancellationToken);

        if (!speciesIsExist)
        {
            _logger.LogWarning("Species with id = {SpeciesId} was not found", breedAndSpeciesId.SpeciesId.Value);
            return (ErrorList)Errors.General.NotFound("species");
        }

        var breedIsExist = await _speciesContract.BreedExistsAsync(breedAndSpeciesId.BreedId, cancellationToken);

        if (!breedIsExist)
        {
            _logger.LogWarning("Breed with id = {BreedId} was not found", breedAndSpeciesId.BreedId);
            return (ErrorList)Errors.General.NotFound("breed");
        }

        return true;
    }
}