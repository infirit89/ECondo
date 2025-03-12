using MediatR;
using ECondo.Api.Extensions;
using ECondo.Application.Data;
using ECondo.Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECondo.Application.Commands.Identity;
using ECondo.Api.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost(nameof(Login))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<JsonHttpResult<TokenResult>, ValidationProblem>>
        Login([FromBody] LoginCommand loginCommand)
    {
        var result = await sender.Send(loginCommand);

        return result switch
        {
            Result<TokenResult, Error>.Success s => TypedResults.Json(s.Data),
            Result<TokenResult, Error>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [HttpPost(nameof(Register))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<Results<Ok, ValidationProblem>>
        Register([FromBody] RegisterCommand registerCommand)
    {
        var result = await sender.Send(registerCommand);
        return result switch
        {
            Result<EmptySuccess, IdentityError[]>.Success => TypedResults.Ok(),
            Result<EmptySuccess, IdentityError[]>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [Authorize]
    [HttpPost(nameof(InvalidateRefreshToken))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<Results<Ok, ValidationProblem>>
        InvalidateRefreshToken([FromBody] InvalidateRefreshTokenRequest request)
    {
        Claim? emailClaim = User.GetEmailClaim();
        InvalidateRefreshTokenCommand command = new(emailClaim is null ? "" : emailClaim.Value, request.RefreshToken);

        var result = await sender.Send(command);

        return result switch
        {
            Result<EmptySuccess, Error>.Success => TypedResults.Ok(),
            Result<EmptySuccess, Error>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [HttpPost(nameof(Refresh))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<Results<JsonHttpResult<TokenResult>, ValidationProblem>>
        Refresh([FromBody] GenerateAccessTokenCommand generateAccessTokenCommand)
    {
        var result = await sender.Send(generateAccessTokenCommand);

        return result switch
        {
            Result<TokenResult, Error>.Success s => TypedResults.Json(s.Data),
            Result<TokenResult, Error>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [HttpPost(nameof(ConfirmEmail))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ValidationProblem>>
        ConfirmEmail([FromBody] ConfirmEmailCommand confirmEmailCommand)
    {
        var result = await sender.Send(confirmEmailCommand);

        return result switch
        {
            Result<EmptySuccess, Error[]>.Success => TypedResults.Ok(),
            Result<EmptySuccess, Error[]>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [HttpPost(nameof(ForgotPassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ValidationProblem>>
        ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordCommand)
    {
        var result = await sender.Send(forgotPasswordCommand);

        return result switch
        {
            Result<EmptySuccess, Error>.Success => TypedResults.Ok(),
            Result<EmptySuccess, Error>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [HttpPost(nameof(ResetPassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ValidationProblem>>
        ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
    {
        var result = await sender.Send(resetPasswordCommand);

        return result switch
        {
            Result<EmptySuccess, Error[]>.Success => TypedResults.Ok(),
            Result<EmptySuccess, Error[]>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [Authorize]
    [HttpPut(nameof(UpdatePassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok, ValidationProblem>>
        UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
    {
        Claim? emailClaim = User.GetEmailClaim();
        UpdatePasswordCommand updatePasswordCommand = new(emailClaim is null ? "" : emailClaim.Value,
            updatePasswordRequest.CurrentPassword, updatePasswordRequest.NewPassword);
        var result = await sender.Send(updatePasswordCommand);

        return result switch
        {
            Result<EmptySuccess, Error[]>.Success => TypedResults.Ok(),
            Result<EmptySuccess, Error[]>.Error e => e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}
