using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity.GenerateAccessToken;
internal class GenerateAccessTokenCommandHandler(
    UserManager<User> userManager,
    IAuthTokenService authService) :
    ICommandHandler<GenerateAccessTokenCommand, TokenResult>
{
    public async Task<Result<TokenResult, Error>> Handle(
        GenerateAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return Result<TokenResult, Error>
                .Fail(UserErrors.InvalidRefreshToken());

        RefreshToken? refreshToken = await authService
            .GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken is null)
            return Result<TokenResult, Error>
                .Fail(UserErrors.InvalidRefreshToken());

        User? user = await userManager
            .FindByIdAsync(refreshToken.UserId.ToString());
        if (user is null)
            return Result<TokenResult, Error>
                .Fail(UserErrors.InvalidUser());

        AccessToken accessToken = authService
            .GenerateAccessTokenAsync(user);

        return Result<TokenResult, Error>
            .Ok(new TokenResult(
                accessToken.Value,
                accessToken.MinutesExpiry, ""));
    }
}
