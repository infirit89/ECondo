using System.ComponentModel.DataAnnotations;
using ECondo.Application.Events.Identity;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

internal sealed class RegisterCommandHandler(
    UserManager<User> userManager,
    IUserStore<User> userStore,
    IdentityErrorDescriber errorDescriber,
    IPublisher publisher) : IRequestHandler<RegisterCommand, Result<EmptySuccess, IdentityError[]>>
{
    public async Task<Result<EmptySuccess, IdentityError[]>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        if (string.IsNullOrEmpty(request.Email) || !_emailAddressAttribute.IsValid(request.Email))
            return Result<EmptySuccess, IdentityError[]>.Fail([errorDescriber.InvalidEmail(request.Email)]);

        if (string.IsNullOrEmpty(request.Username))
            return Result<EmptySuccess, IdentityError[]>.Fail([errorDescriber.InvalidUserName(request.Username)]);

        User user = new User();
        await userStore.SetUserNameAsync(user, request.Username, cancellationToken);
        await emailStore.SetEmailAsync(user, request.Email, cancellationToken);

        // TODO: email confirmation
        //await emailStore.SetEmailConfirmedAsync(user, true, cancellationToken);
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result<EmptySuccess, IdentityError[]>.Fail(result.Errors.ToArray());

        await publisher.Publish(new UserRegisteredEvent(user, request.ReturnUri), cancellationToken);

        return Result<EmptySuccess, IdentityError[]>.Ok();
    }

    private readonly EmailAddressAttribute _emailAddressAttribute = new();
}
