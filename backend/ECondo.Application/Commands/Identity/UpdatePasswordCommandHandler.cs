using ECondo.Application.Extensions;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

internal sealed class UpdatePasswordCommandHandler(
    UserManager<User> userManager,
    IUserContext userContext) : IRequestHandler<UpdatePasswordCommand, Result<EmptySuccess, Error[]>>
{
    public async Task<Result<EmptySuccess, Error[]>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<EmptySuccess, Error[]>.Fail([UserErrors.InvalidUser()]);

        User? user = await userManager.FindByIdAsync(userContext.UserId.ToString()!);

        if(user is null)
            return Result<EmptySuccess, Error[]>.Fail([UserErrors.InvalidUser()]);

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if(!result.Succeeded)
            return Result<EmptySuccess, Error[]>.Fail(result.Errors.ToErrorArray());

        return Result<EmptySuccess, Error[]>.Ok();
    }
}