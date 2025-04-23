using ECondo.Api.Extensions;
using ECondo.Application.Commands.PropertyOccupants.AddToProperty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyOccupantController(ISender sender) 
    : ControllerBase
{
    [Authorize]
    [HttpPost(nameof(Create))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> Create(
        [FromBody] AddOccupantToPropertyCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
}