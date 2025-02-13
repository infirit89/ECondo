using System.ComponentModel.DataAnnotations;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;

internal sealed class RegisterCommandHandler(
    UserManager<User> userManager,
    IUserStore<User> userStore,
    IdentityErrorDescriber errorDescriber) : IRequestHandler<RegisterCommand, Result<Empty, IdentityError[]>>
{
    public async Task<Result<Empty, IdentityError[]>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        if (string.IsNullOrEmpty(request.Email) || !_emailAddressAttribute.IsValid(request.Email))
            return Result<Empty, IdentityError[]>.Fail([errorDescriber.InvalidEmail(request.Email)]);

        if(string.IsNullOrEmpty(request.Username))
            return Result<Empty, IdentityError[]>.Fail([errorDescriber.InvalidUserName(request.Username)]);

        User user = new User();
        await userStore.SetUserNameAsync(user, request.Username, cancellationToken);
        await emailStore.SetEmailAsync(user, request.Email, cancellationToken);

        // TODO: email confirmation
        await emailStore.SetEmailConfirmedAsync(user, true, cancellationToken);
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result<Empty, IdentityError[]>.Fail(result.Errors.ToArray());

        return Result<Empty, IdentityError[]>.Ok();
    }

    private readonly EmailAddressAttribute _emailAddressAttribute = new();
}
