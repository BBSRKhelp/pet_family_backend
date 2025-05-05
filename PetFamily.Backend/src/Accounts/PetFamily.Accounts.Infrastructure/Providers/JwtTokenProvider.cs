using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application.Interfaces;
using PetFamily.Accounts.Domain.DataModels;
using PetFamily.Accounts.Infrastructure.Options;

namespace PetFamily.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly ILogger<JwtTokenProvider> _logger;
    private readonly JwtOptions _jwtOptions;

    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        ILogger<JwtTokenProvider> logger)
    {
        _jwtOptions = options.Value;
        _logger = logger;
    }

    public string GenerateAccessToken(User user)
    {
        _logger.LogInformation("Generating access token");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        ];

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetime),
            signingCredentials: signingCredentials,
            claims: claims);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        _logger.LogInformation("Successfully generated access token");

        return stringToken;
    }
}