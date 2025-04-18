using ECondo.Api.Extensions;
using ECondo.Application.Commands.Profiles.Create;
using ECondo.Application.Commands.Profiles.Update;
using ECondo.Application.Data;
using ECondo.Application.Queries.Profiles.GetBrief;
using ECondo.Application.Queries.Profiles.GetForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost(nameof(Create))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        Create([FromBody] CreateProfileCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpGet(nameof(GetBriefProfile))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BriefProfileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        GetBriefProfile()
    {
        var result = await sender.Send(new GetBriefProfileQuery());

        return result.Match(data => TypedResults.Json(data), CustomResults.Problem);
    }

    [Authorize]
    [HttpPut(nameof(UpdateProfile))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        UpdateProfile([FromBody] UpdateProfileCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpGet(nameof(GetProfile))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        GetProfile()
    {
        var result = await sender.Send(new GetProfileQuery());

        return result.Match(data => TypedResults.Json(data), CustomResults.Problem);
    }
}