using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;
internal sealed class ConfirmEmailCommandHandler(UserManager<User> userManager) : IRequestHandler<ConfirmEmailCommand, Result<EmptySuccess, IdentityError[]>>
{
    public async Task<Result<EmptySuccess, IdentityError[]>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            var error = UserErrors.InvalidUser(request.Email);
            return Result<EmptySuccess, IdentityError[]>.Fail([
                new IdentityError
                {
                    Code = error.Code,
                    Description = error.Description,
                }
            ]);
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if(!result.Succeeded)
            return Result<EmptySuccess, IdentityError[]>.Fail(result.Errors.ToArray());

        return Result<EmptySuccess, IdentityError[]>.Ok();
    }
}
