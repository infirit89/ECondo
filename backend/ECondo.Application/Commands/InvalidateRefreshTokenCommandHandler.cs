using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;

internal sealed class InvalidateRefreshTokenCommandHandler(
    UserManager<User> userManager,
    IAuthTokenService authService)
    : IRequestHandler<InvalidateRefreshTokenCommand, Result<Empty, IdentityError>>
{
    public async Task<Result<Empty, IdentityError>> Handle(InvalidateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken) || await authService.RefreshTokenExistsAsync(request.RefreshToken))
            return Result<Empty, IdentityError>.Fail(UserErrors.InvalidRefreshToken());

        User? user = await userManager.FindByNameAsync(request.Username);
        if(user is null)
            return Result<Empty, IdentityError>.Fail(UserErrors.InvalidUser(request.Username));

        await authService.RemoveRefreshTokenAsync(request.RefreshToken);
        return Result<Empty, IdentityError>.Ok();
    }
}
