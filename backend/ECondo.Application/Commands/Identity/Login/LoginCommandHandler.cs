using ECondo.Application.Data;
using ECondo.Application.Extensions;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity.Login;
internal class LoginCommandHandler(
    UserManager<User> userManager,
    IdentityErrorDescriber errorDescriber,
    IAuthTokenService authTokenService)
    : ICommandHandler<LoginCommand, TokenResult>
{
    public async Task<Result<TokenResult, Error>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        User? user = await userManager
            .FindUserByEmailOrNameAsync(request.Email);

        if (user is null)
            return Result<TokenResult, Error>
                .Fail(UserErrors.InvalidUser(request.Email));

        if (!await userManager
                .CheckPasswordAsync(user, request.Password))
        {
            var passwordMismatchError =
                errorDescriber.PasswordMismatch();
            return Result<TokenResult, Error>.Fail(new[]
            {
                passwordMismatchError
            }.ToValidationError());
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
            return Result<TokenResult, Error>
                .Fail(UserErrors.EmailNotConfirmed());

        AccessToken accessToken =
            authTokenService.GenerateAccessTokenAsync(user);
        RefreshToken refreshToken =
            authTokenService.GenerateRefreshTokenAsync(user);

        await authTokenService.StoreRefreshTokenAsync(refreshToken);

        return Result<TokenResult, Error>.Ok(
            new TokenResult(
                accessToken.Value,
                accessToken.MinutesExpiry,
                refreshToken.Value));
    }
}
