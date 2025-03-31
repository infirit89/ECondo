using ECondo.Api.Data.Profile;
using ECondo.Api.Extensions;
using ECondo.Application.Commands.Profile;
using ECondo.Application.Data;
using ECondo.Application.Queries.Profile;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok, ValidationProblem, UnauthorizedHttpResult>>
            Create([FromBody] CreateProfileRequest request)
        {
            CreateProfileCommand createProfileCommand =
                new(User.GetEmail() ?? "", request.FirstName, request.MiddleName, request.LastName, request.Phone);

            var result = await sender.Send(createProfileCommand);

            return result switch
            {
                Result<EmptySuccess, Error>.Success => TypedResults.Ok(),
                Result<EmptySuccess, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        [Authorize]
        [HttpGet(nameof(GetBriefProfile))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefProfileResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<JsonHttpResult<BriefProfileResult>, ValidationProblem, UnauthorizedHttpResult>>
            GetBriefProfile()
        {
            var result = await sender.Send(new GetBriefProfileQuery());

            return result switch
            {
                Result<BriefProfileResult, Error>.Success s => TypedResults.Json(s.Data),
                Result<BriefProfileResult, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        [Authorize]
        [HttpPut(nameof(UpdateProfile))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok, ValidationProblem, UnauthorizedHttpResult>>
            UpdateProfile([FromBody] UpdateProfileCommand request)
        {
            var result = await sender.Send(request);

            return result switch
            {
                Result<EmptySuccess, Error>.Success => TypedResults.Ok(),
                Result<EmptySuccess, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        [Authorize]
        [HttpGet(nameof(GetProfile))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<JsonHttpResult<ProfileResult>, ValidationProblem, UnauthorizedHttpResult>>
            GetProfile()
        {
            var result = await sender.Send(new GetProfileQuery());
            return result switch
            {
                Result<ProfileResult, Error>.Success s => TypedResults.Json(s.Data),
                Result<ProfileResult, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}
