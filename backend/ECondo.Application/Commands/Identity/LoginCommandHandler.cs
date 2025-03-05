using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;
internal class LoginCommandHandler(
    UserManager<User> userManager,
    IdentityErrorDescriber errorDescriber,
    IAuthTokenService authTokenService) : IRequestHandler<LoginCommand, Result<TokenResult, Error>>
{
    public async Task<Result<TokenResult, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email)
                     ?? await userManager.FindByNameAsync(request.Email);

        if (user is null)
            return Result<TokenResult, Error>.Fail(UserErrors.InvalidUser(request.Email));

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            var passwordMismatchError = errorDescriber.PasswordMismatch();
            return Result<TokenResult, Error>.Fail(new Error
            {
                Code = passwordMismatchError.Code,
                Description = passwordMismatchError.Description,
            });
        }

        if(!await userManager.IsEmailConfirmedAsync(user))
            return Result<TokenResult, Error>.Fail(UserErrors.EmailNotConfirmed());

        AccessToken accessToken = authTokenService.GenerateAccessTokenAsync(user);
        RefreshToken refreshToken = authTokenService.GenerateRefreshTokenAsync(user);

        await authTokenService.StoreRefreshTokenAsync(refreshToken);

        return Result<TokenResult, Error>.Ok(new TokenResult(accessToken.Value, accessToken.MinutesExpiry, refreshToken.Value));
    }
}
