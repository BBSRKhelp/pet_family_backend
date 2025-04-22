using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Queries.Pet.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;