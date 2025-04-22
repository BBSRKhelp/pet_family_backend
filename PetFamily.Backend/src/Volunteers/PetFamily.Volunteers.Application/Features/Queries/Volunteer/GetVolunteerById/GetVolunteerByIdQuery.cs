using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Queries.Volunteer.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;