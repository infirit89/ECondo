using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;
internal class LoginCommandHandler(
    UserManager<User> userManager, 
    IdentityErrorDescriber errorDescriber, 
    IAuthTokenService authTokenService) : IRequestHandler<LoginCommand, Result<TokenResult, IdentityError>>
{
    public async Task<Result<TokenResult, IdentityError>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email)
                     ?? await userManager.FindByNameAsync(request.Email);

        if (user is null)
            return Result<TokenResult, IdentityError>.Fail(errorDescriber.InvalidUserName(request.Email));

        if(!await userManager.CheckPasswordAsync(user, request.Password))
            return Result<TokenResult, IdentityError>.Fail(errorDescriber.PasswordMismatch());

        string accessToken = authTokenService.GenerateAccessTokenAsync(user);
        RefreshToken refreshToken = authTokenService.GenerateRefreshTokenAsync(user);

        await authTokenService.StoreRefreshTokenAsync(refreshToken);

        return Result<TokenResult, IdentityError>.Ok(new TokenResult(accessToken, refreshToken.Value));
    }
}
