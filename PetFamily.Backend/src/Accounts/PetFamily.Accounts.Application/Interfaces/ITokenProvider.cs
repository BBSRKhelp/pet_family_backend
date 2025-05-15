using PetFamily.Accounts.Domain.DataModels;

namespace PetFamily.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}