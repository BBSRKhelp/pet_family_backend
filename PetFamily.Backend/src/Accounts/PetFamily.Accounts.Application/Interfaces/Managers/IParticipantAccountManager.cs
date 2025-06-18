using PetFamily.Accounts.Domain.DataModels;

namespace PetFamily.Accounts.Application.Interfaces.Managers;

public interface IParticipantAccountManager
{
    Task CreateParticipantAccountAsync(ParticipantAccount participantAccount,
        CancellationToken cancellationToken = default);
}