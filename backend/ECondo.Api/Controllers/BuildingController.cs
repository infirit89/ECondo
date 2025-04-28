using ECondo.Api.Extensions;
using ECondo.Application.Commands.Buildings.Delete;
using ECondo.Application.Commands.Buildings.RegisterEntrance;
using ECondo.Application.Data;
using ECondo.Application.Queries.Buildings.GetAll;
using ECondo.Application.Queries.Buildings.GetForUser;
using ECondo.Application.Queries.Buildings.IsUserIn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IResult>
        RegisterBuildingEntrance(
            [FromBody] RegisterBuildingEntranceCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
    
    [Authorize]
    [HttpDelete(nameof(Delete))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, 
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IResult>
        Delete(
            [FromQuery] DeleteBuildingEntranceCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpGet(nameof(GetBuildingsForUser))]
    [ProducesResponseType(
        StatusCodes.Status200OK, 
        Type = typeof(PagedListResponse<BuildingResult>))]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest, 
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        GetBuildingsForUser([FromQuery] GetBuildingsForUserQuery request)
    {
        var result = await sender.Send(request);
        return result.Match(
            data => TypedResults.Json(data.ToPagedListResponse()), 
            CustomResults.Problem);
    }

    [Authorize]
    [HttpGet(nameof(IsEntranceManager))]
    [ProducesResponseType(
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        IsEntranceManager([FromQuery] IsUserEntranceManagerQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
    
    [Authorize]
    [HttpGet(nameof(GetAll))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(PagedListResponse<BuildingResult>))]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        GetAll([FromQuery] GetAllBuildingsQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(
            data => TypedResults.Json(data.ToPagedListResponse()), 
            CustomResults.Problem);
    }
}