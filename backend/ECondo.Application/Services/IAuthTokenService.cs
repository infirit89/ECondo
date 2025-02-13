
using ECondo.Domain.Shared;
using ECondo.Domain.Users;

namespace ECondo.Application.Services;
public interface IAuthTokenService
{
    string GenerateAccessTokenAsync(User user);
    RefreshToken GenerateRefreshTokenAsync(User user);
    Task StoreRefreshTokenAsync(RefreshToken refreshToken);
    Task RemoveRefreshTokenAsync(string tokenValue);
    Task<RefreshToken?> GetRefreshTokenAsync(string tokenValue);
    Task<bool> RefreshTokenExistsAsync(string tokenValue);
}
