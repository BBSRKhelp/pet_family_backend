using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteer.Application.Features.Queries.Volunteer.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;