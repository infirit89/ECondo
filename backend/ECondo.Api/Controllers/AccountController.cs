using ECondo.Api.Data;
using ECondo.Api.Extensions;
using ECondo.Application.Commands;
using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                Result<TokenResult, IdentityError>.Success s => TypedResults.Json(s.Data),
                Result<TokenResult, IdentityError>.Error e => e.Data.ToValidationProblem(),
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
                registerUserRequest.Password);

            var result = await sender.Send(command);
            return result switch
            {
                Result<Empty, IdentityError[]>.Success => TypedResults.Ok(),
                Result<Empty, IdentityError[]>.Error e => e.Data.ToValidationProblem(),
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
                Result<Empty, IdentityError>.Success => TypedResults.Ok(),
                Result<Empty, IdentityError>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        public async Task<Results<JsonHttpResult<TokenResult>, ValidationProblem>>
            GenerateAccessToken([FromBody] GenerateAccessTokenRequest accessTokenRequest)
        {
            GenerateAccessTokenCommand command = new(accessTokenRequest.RefreshToken);
            var result = await sender.Send(command);

            return result switch
            {
                Result<TokenResult, IdentityError>.Success s => TypedResults.Json(s.Data),
                Result<TokenResult, IdentityError>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}
