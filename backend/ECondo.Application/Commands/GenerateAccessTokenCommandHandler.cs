using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;
internal class GenerateAccessTokenCommandHandler(
    UserManager<User> userManager,
    IAuthTokenService authService) : 
    IRequestHandler<GenerateAccessTokenCommand, Result<TokenResult, IdentityError>>
{
    public async Task<Result<TokenResult, IdentityError>> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.RefreshToken))
            return Result<TokenResult, IdentityError>.Fail(UserErrors.InvalidRefreshToken());

        RefreshToken? refreshToken = await authService.GetRefreshTokenAsync(request.RefreshToken);
        if(refreshToken is null)
            return Result<TokenResult, IdentityError>.Fail(UserErrors.InvalidRefreshToken());

        User? user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user is null)
            return Result<TokenResult, IdentityError>.Fail(UserErrors.InvalidUser());

        string accessToken = authService.GenerateAccessTokenAsync(user);

        return Result<TokenResult, IdentityError>.Ok(new TokenResult(accessToken, ""));
    }
}
