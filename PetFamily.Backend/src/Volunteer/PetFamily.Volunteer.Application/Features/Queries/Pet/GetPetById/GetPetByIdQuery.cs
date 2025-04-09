using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteer.Application.Features.Queries.Pet.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;