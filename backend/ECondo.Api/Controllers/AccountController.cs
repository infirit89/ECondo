using MediatR;
using ECondo.Api.Extensions;
using ECondo.Application.Data;
using ECondo.Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECondo.Application.Commands.Identity;
using ECondo.Api.Data.Identity;

namespace ECondo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(ISender sender) : ControllerBase
    {
        [HttpPost(nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Results<JsonHttpResult<TokenResult>, ValidationProblem>>
            Login([FromBody] LoginUserRequest loginUserRequest)
        {
            LoginCommand command = new(loginUserRequest.Email, loginUserRequest.Password);
            var result = await sender.Send(command);

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
            Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            RegisterCommand command = new(registerUserRequest.Email, registerUserRequest.Username,
                registerUserRequest.Password, registerUserRequest.ReturnUri);

            var result = await sender.Send(command);
            return result switch
            {
                Result<EmptySuccess, IdentityError[]>.Success => TypedResults.Ok(),
                Result<EmptySuccess, IdentityError[]>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        [HttpPost(nameof(InvalidateRefreshToken))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
        public async Task<Results<Ok, ValidationProblem>>
            InvalidateRefreshToken([FromBody] InvalidateRefreshTokenRequest request)
        {
            InvalidateRefreshTokenCommand command = new(request.Username, request.RefreshToken);

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
            Refresh([FromBody] GenerateAccessTokenRequest accessTokenRequest)
        {
            GenerateAccessTokenCommand command = new(accessTokenRequest.RefreshToken);
            var result = await sender.Send(command);

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
            ConfirmEmail([FromBody] ConfirmEmailRequest confirmEmailRequest)
        {
            ConfirmEmailCommand confirmEmailCommand = new(confirmEmailRequest.Token, confirmEmailRequest.Email);

            var result = await sender.Send(confirmEmailCommand);

            return result switch
            {
                Result<EmptySuccess, IdentityError[]>.Success => TypedResults.Ok(),
                Result<EmptySuccess, IdentityError[]>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}
