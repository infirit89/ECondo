using ECondo.Api.Extensions;
using ECondo.Application.Commands.Properties.Create;
using ECondo.Application.Commands.Properties.Delete;
using ECondo.Application.Commands.Properties.Update;
using ECondo.Application.Queries.Properties.GetInBuilding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpGet(nameof(GetPropertiesInBuilding))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(PagedListResponse<BriefPropertyResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult>
        GetPropertiesInBuilding(
            [FromQuery] GetPropertiesInBuildingQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(
            data => TypedResults.Json(data.ToPagedListResponse()),
            CustomResults.Problem);
    }

    [Authorize]
    [HttpPost(nameof(Create))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> Create(
        [FromBody] CreatePropertyCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }

    [Authorize]
    [HttpPut(nameof(Update))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> Update(
        [FromBody] UpdatePropertyCommand request)
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> Delete(
        [FromQuery] DeletePropertyCommand request)
    {
        var result = await sender.Send(request);
        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
}