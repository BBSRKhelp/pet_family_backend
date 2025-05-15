using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.Ids;

namespace PetFamily.Volunteers.Application.Interfaces;

public interface IVolunteersRepository
{ 
    Task<Guid> AddAsync(Domain.Volunteer volunteer, CancellationToken cancellationToken = default);
    Guid Delete(Domain.Volunteer volunteer);
    Task<Result<Domain.Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    Task<Result<Domain.Volunteer, Error>> GetByPhoneAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);
    Task<Result<Domain.Volunteer, Error>> GetByEmailAsync(Email requestEmail, CancellationToken cancellationToken);
}