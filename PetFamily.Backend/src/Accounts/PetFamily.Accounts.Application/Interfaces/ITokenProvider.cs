using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    AccessTokenResult GenerateAccessToken(User user);
    Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaimsFromTokenAsync(string accessToken);
}