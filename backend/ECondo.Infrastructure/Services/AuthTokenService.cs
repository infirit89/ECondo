using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECondo.Infrastructure.Services;

internal class AuthTokenService : IAuthTokenService
{
    public AuthTokenService(
        IOptions<JwtSettings> jwtOptions, 
        IOptions<RefreshTokenSettings> refreshTokenOptions,
        ICacheRepository cacheRepository)
    {
        _jwtSettings = jwtOptions.Value;
        _refreshTokenSettings = refreshTokenOptions.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        _cacheRepository = cacheRepository;
        _refreshTokenEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddDays(_refreshTokenSettings.DaysExpiry)
        };
    }

    public AccessToken GenerateAccessTokenAsync(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.UserName!),
        ];
        var signingCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesExpiry),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = signingCredentials,
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return new AccessToken
        {
            Value = jwtTokenHandler.WriteToken(token),
            MinutesExpiry = _jwtSettings.MinutesExpiry,
        };
    }

    public RefreshToken GenerateRefreshTokenAsync(User user)
    {
        using var generator = RandomNumberGenerator.Create();
        var randomNumber = new byte[_refreshTokenSettings.Length];
        generator.GetBytes(randomNumber);

        RefreshToken token = new()
        {
            Value = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(_refreshTokenSettings.DaysExpiry),
            UserId = user.Id
        };
        return token;
    }

    private string GetRefreshTokenKey(string tokenValue) => $"refreshtoken-{tokenValue}";

    public Task StoreRefreshTokenAsync(RefreshToken refreshToken)
        => _cacheRepository.StoreAsync(GetRefreshTokenKey(refreshToken.Value), refreshToken, _refreshTokenEntryOptions);

    public Task RemoveRefreshTokenAsync(string tokenValue)
        => _cacheRepository.RemoveAsync(GetRefreshTokenKey(tokenValue));

    public Task<RefreshToken?> GetRefreshTokenAsync(string tokenValue)
        => _cacheRepository.GetAsync<RefreshToken>(GetRefreshTokenKey(tokenValue));

    public async Task<bool> RefreshTokenExistsAsync(string tokenValue)
        => await _cacheRepository.GetAsync<RefreshToken>(GetRefreshTokenKey(tokenValue)) is not null;


    private readonly JwtSettings _jwtSettings;
    private readonly RefreshTokenSettings _refreshTokenSettings;
    private readonly SymmetricSecurityKey _key;
    private readonly DistributedCacheEntryOptions _refreshTokenEntryOptions;
    private readonly ICacheRepository _cacheRepository;
}
