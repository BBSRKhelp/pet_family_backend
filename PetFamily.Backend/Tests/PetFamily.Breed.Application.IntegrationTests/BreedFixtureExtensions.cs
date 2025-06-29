using AutoFixture;
using PetFamily.Species.Application.Features.Queries.GetBreedsByIdSpecies;

namespace PetFamily.Breed.Application.IntegrationTests;

public static class BreedFixtureExtensions
{
    public static GetBreedsByIdSpeciesQuery BuildGetBreedsByIdSpeciesQuery(
        this Fixture fixture,
        Guid speciesId,
        int pageNumber,
        int pageSize)
    {
        return fixture.Build<GetBreedsByIdSpeciesQuery>()
            .With(b => b.SpeciesId, speciesId)
            .With(b => b.PageNumber, pageNumber)
            .With(b => b.PageSize, pageSize)
            .With(b => b.Name, (string?)null)
            .With(b => b.SortBy, "id")
            .With(b => b.SortDirection, "ASC")
            .Create();
    }
}