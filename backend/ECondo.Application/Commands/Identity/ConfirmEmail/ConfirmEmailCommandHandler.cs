using ECondo.Application.Extensions;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity.ConfirmEmail;
internal sealed class ConfirmEmailCommandHandler(
    UserManager<User> userManager) 
    : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<Result<EmptySuccess, Error>> 
        Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
    {
        User? user = await userManager
            .FindByEmailAsync(request.Email);

        if (user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser(request.Email));

        var result = await userManager
            .ConfirmEmailAsync(user, request.Token);
        if(!result.Succeeded)
            return Result<EmptySuccess, Error>
                .Fail(result.Errors.ToValidationError());

        return Result<EmptySuccess, Error>.Ok();
    }
}
