using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

internal sealed class ResetPasswordCommandHandler(UserManager<User> userManager) : IRequestHandler<ResetPasswordCommand, Result<EmptySuccess, IdentityError[]>>
{
    public async Task<Result<EmptySuccess, IdentityError[]>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            var invalidError = UserErrors.InvalidUser(request.Email);
            IdentityError error = new()
            {
                Code = invalidError.Code,
                Description = invalidError.Description,
            };
            return Result<EmptySuccess, IdentityError[]>.Fail([error]);
        }


        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if(!result.Succeeded)
            return Result<EmptySuccess, IdentityError[]>.Fail(result.Errors.ToArray());

        return Result<EmptySuccess, IdentityError[]>.Ok();
    }
}
