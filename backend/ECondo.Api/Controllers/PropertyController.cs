using ECondo.Api.Extensions;
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
}