using ECondo.Application.Extensions;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    UserManager<User> userManager) 
    : ICommandHandler <ResetPasswordCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser(request.Email));

        var result = await userManager.ResetPasswordAsync(user,
            request.Token,
            request.NewPassword);

        if(!result.Succeeded)
            return Result<EmptySuccess, Error>
                .Fail(result.Errors.ToValidationError());

        return Result<EmptySuccess, Error>.Ok();
    }
}
