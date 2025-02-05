
using ECondo.Domain.Users;

namespace ECondo.Application.Services;
public interface IAuthTokenService
{
    string GenerateAccessTokenAsync(User user);
}
