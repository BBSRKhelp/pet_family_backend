using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;