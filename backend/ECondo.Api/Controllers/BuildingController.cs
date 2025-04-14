using ECondo.Api.Extensions;
using ECondo.Application.Commands.Building;
using ECondo.Application.Data;
using ECondo.Application.Queries.Building;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuildingController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost(nameof(RegisterBuildingEntrance))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, 
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<
            Results<Ok, ValidationProblem, UnauthorizedHttpResult>>
        RegisterBuildingEntrance(
            [FromBody] RegisterBuildingEntranceCommand request)
    {
        var result = await sender.Send(request);

        return result switch
        {
            Result<EmptySuccess, Error>.Success =>
                TypedResults.Ok(),

            Result<EmptySuccess, Error>.Error e =>
                e.Data.ToValidationProblem(),

            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [Authorize]
    [HttpGet(nameof(GetBuildingsForUser))]
    [ProducesResponseType(
        StatusCodes.Status200OK, 
        Type = typeof(BuildingResult[]))]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest, 
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<
            Results<
                JsonHttpResult<BuildingResult[]>,
                ValidationProblem,
                UnauthorizedHttpResult>>
        GetBuildingsForUser()
    {
        var result = await sender
            .Send(new GetBuildingsForUserQuery());

        return result switch
        {
            Result<BuildingResult[], Error>.Success s => 
                TypedResults.Json(s.Data),

            Result<BuildingResult[], Error>.Error e => 
                e.Data.ToValidationProblem(),

            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }

    [Authorize]
    [HttpGet(nameof(IsInBuilding))]
    [ProducesResponseType(
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok, UnauthorizedHttpResult, ValidationProblem>>
        IsInBuilding([FromQuery] IsUserInBuildingQuery request)
    {
        var result = await sender.Send(request);

        return result switch
        {
            Result<EmptySuccess, Error>.Success =>
                TypedResults.Ok(),

            Result<EmptySuccess, Error>.Error e =>
                e.Data.ToValidationProblem(),
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}