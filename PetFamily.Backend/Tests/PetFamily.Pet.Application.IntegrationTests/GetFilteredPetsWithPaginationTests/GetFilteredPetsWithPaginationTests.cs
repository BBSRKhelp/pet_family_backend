using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Volunteers.Application.Features.Queries.Pet.GetFilteredPetsWithPagination;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Pet.Application.IntegrationTests.GetFilteredPetsWithPaginationTests;

public class GetFilteredPetsWithPaginationTests : PetTestsBase
{
    private readonly IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery> _sut;
    
    protected GetFilteredPetsWithPaginationTests(PetTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<PagedList<PetDto>, 
            GetFilteredPetsWithPaginationQuery>>();
    }
}