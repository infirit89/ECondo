﻿using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace ECondo.Application.Commands.Identity.ForgotPassword;
internal sealed class ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    IEmailService emailService) 
    : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        User? user = await userManager
            .FindByEmailAsync(request.Username);

        if(user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser(request.Username));

        string token = await userManager
            .GeneratePasswordResetTokenAsync(user);

        var queryParams = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", request.Username },
        };
        string returnUrl = QueryHelpers
            .AddQueryString(request.ReturnUri, queryParams);

        await emailService
            .SendPasswordResetMail(user.Email!, returnUrl);
        return Result<EmptySuccess, Error>.Ok();
    }
}
