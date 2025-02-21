using ECondo.Api.Data.Profile;
using ECondo.Api.Extensions;
using ECondo.Application.Commands.Profile;
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
            CreateProfileCommand createProfileCommand =
                new(request.Username, request.FirstName, request.MiddleName, request.LastName);

            var result = await sender.Send(createProfileCommand);

            return result switch
            {
                Result<EmptySuccess, Error>.Success => TypedResults.Ok(),
                Result<EmptySuccess, Error>.Error e => e.Data.ToValidationProblem(),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}
