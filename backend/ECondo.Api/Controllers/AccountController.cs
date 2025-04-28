using MediatR;
using ECondo.Api.Extensions;
using ECondo.Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECondo.Application.Commands.Identity.ConfirmEmail;
using ECondo.Application.Commands.Identity.Delete;
using ECondo.Application.Commands.Identity.ForgotPassword;
using ECondo.Application.Commands.Identity.GenerateAccessToken;
using ECondo.Application.Commands.Identity.InvalidateRefreshToken;
using ECondo.Application.Commands.Identity.Login;
using ECondo.Application.Commands.Identity.Register;
using ECondo.Application.Commands.Identity.ResetPassword;
using ECondo.Application.Commands.Identity.UpdatePassword;
using ECondo.Application.Queries.Identity.IsInRole;

namespace ECondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost(nameof(Login))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult>
        Login([FromBody] LoginCommand loginCommand)
    {
        var result = await sender.Send(loginCommand);

        return result.Match(data => TypedResults.Json(data), CustomResults.Problem);
    }

    [HttpPost(nameof(Register))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult>
        Register([FromBody] RegisterCommand registerRequest)
    {
        var result = await sender.Send(registerRequest);
        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpPost(nameof(InvalidateRefreshToken))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult>
        InvalidateRefreshToken([FromBody] InvalidateRefreshTokenCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [HttpPost(nameof(Refresh))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult>
        Refresh([FromBody] GenerateAccessTokenCommand generateAccessTokenCommand)
    {
        var result = await sender.Send(generateAccessTokenCommand);

        return result.Match(data => TypedResults.Json(data), CustomResults.Problem);
    }

    [HttpPost(nameof(ConfirmEmail))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult>
        ConfirmEmail([FromBody] ConfirmEmailCommand confirmEmailCommand)
    {
        var result = await sender.Send(confirmEmailCommand);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [HttpPost(nameof(ForgotPassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult>
        ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordCommand)
    {
        var result = await sender.Send(forgotPasswordCommand);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [HttpPost(nameof(ResetPassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult>
        ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
    {
        var result = await sender.Send(resetPasswordCommand);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpPut(nameof(UpdatePassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult>
        UpdatePassword([FromBody] UpdatePasswordCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
    
    [Authorize]
    [HttpGet(nameof(IsInRole))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult>
        IsInRole([FromQuery] IsUserInRoleQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
    
    [Authorize]
    [HttpDelete(nameof(Delete))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult>
        Delete([FromQuery] DeleteUserCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
}
