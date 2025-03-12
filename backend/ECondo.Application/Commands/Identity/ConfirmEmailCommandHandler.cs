using ECondo.Application.Extensions;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;
internal sealed class ConfirmEmailCommandHandler(UserManager<User> userManager) : IRequestHandler<ConfirmEmailCommand, Result<EmptySuccess, Error[]>>
{
    public async Task<Result<EmptySuccess, Error[]>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<EmptySuccess, Error[]>.Fail([UserErrors.InvalidUser(request.Email)]);

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if(!result.Succeeded)
            return Result<EmptySuccess, Error[]>.Fail(result.Errors.ToErrorArray());

        return Result<EmptySuccess, Error[]>.Ok();
    }
}
