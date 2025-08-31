using System.ComponentModel.DataAnnotations;
using ECondo.Application.Events.Identity;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Identity.Register;

internal sealed class RegisterCommandHandler(
    UserManager<User> userManager,
    IUserStore<User> userStore,
    IApplicationDbContext dbContext,
    IdentityErrorDescriber errorDescriber,
    IPublisher publisher) 
    : ICommandHandler<RegisterCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var emailStore = (IUserEmailStore<User>)userStore;
        if (string.IsNullOrEmpty(request.Email) ||
            !_emailAddressAttribute.IsValid(request.Email))
        {
            return Result<EmptySuccess, Error>.Fail(new[]
            {
                errorDescriber.InvalidEmail(request.Email)
            }.ToValidationError());
        }

        var user = await dbContext
            .Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == request.Email, 
                cancellationToken: cancellationToken);

        if (user is not null)
        {
            return Result<EmptySuccess, Error>.Fail(new []
            {
                errorDescriber.DuplicateEmail(request.Email)
            }.ToValidationError());
        }
        
        if (string.IsNullOrEmpty(request.Username))
        {
            return Result<EmptySuccess, Error>.Fail(new []
            {
                errorDescriber.InvalidUserName(request.Username)
            }.ToValidationError());
        }
        
        user = new User();
        await userStore.SetUserNameAsync(user,
            request.Username,
            cancellationToken);

        await emailStore.SetEmailAsync(user,
            request.Email,
            cancellationToken);

        var result = await userManager
            .CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return Result<EmptySuccess, Error>
                .Fail(result.Errors.ToValidationError());

        await publisher.Publish(notification:
            new UserRegisteredEvent(
                user,
                request.ReturnUri),
            cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }

    private readonly EmailAddressAttribute _emailAddressAttribute 
        = new();
}
