using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;

namespace ECondo.Application.Commands.Identity.InvalidateRefreshToken;

internal sealed class InvalidateRefreshTokenCommandHandler(
    IAuthTokenService authService,
    IUserContext userContext)
    : ICommandHandler<InvalidateRefreshTokenCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        InvalidateRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken) || 
            !await authService
                .RefreshTokenExistsAsync(request.RefreshToken))
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidRefreshToken());

        if (userContext.UserId is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser());

        await authService
            .RemoveRefreshTokenAsync(request.RefreshToken);
        return Result<EmptySuccess, Error>.Ok();
    }
}
