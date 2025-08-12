using ECondo.Application.Extensions;
using ECondo.Application.Services;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity.UpdatePassword;

internal sealed class UpdatePasswordCommandHandler(
    UserManager<User> userManager,
    IUserContext userContext) 
    : ICommandHandler<UpdatePasswordCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken)
    {
        User? user = await userManager
            .FindByIdAsync(userContext.UserId.ToString());

        if(user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser());

        var result = await userManager.ChangePasswordAsync(user,
            request.CurrentPassword,
            request.NewPassword);

        if(!result.Succeeded)
            return Result<EmptySuccess, Error>
                .Fail(result.Errors.ToValidationError());

        return Result<EmptySuccess, Error>.Ok();
    }
}