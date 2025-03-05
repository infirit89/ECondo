using System.Security.Claims;
using ECondo.Api.Data.Profile;
using ECondo.Api.Extensions;
using ECondo.Application.Commands.Profile;
using ECondo.Application.Data;
using ECondo.Application.Queries.Profile;
using ECondo.Domain.Shared;
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
            Claim? emailClaim = User.GetEmailClaim();

            CreateProfileCommand createProfileCommand =
                new(emailClaim is null ? "" : emailClaim.Value, request.FirstName, request.MiddleName, request.LastName, request.Phone);

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
            var emailClaim = User.GetEmailClaim();

            GetBriefProfileQuery getBriefProfileQuery = new(emailClaim is null ? "" : emailClaim.Value);

            var result = await sender.Send(getBriefProfileQuery);

            return result switch
            {
                Result<BriefProfileResult, Error>.Success s => TypedResults.Json(s.Data),
                Result<BriefProfileResult, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}
