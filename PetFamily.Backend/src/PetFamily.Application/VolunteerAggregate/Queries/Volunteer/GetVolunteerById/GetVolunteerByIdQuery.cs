using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Queries.Volunteer.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;