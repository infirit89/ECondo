using ECondo.Api.Extensions;
using ECondo.Application.Commands.PropertyOccupants.AcceptInvitation;
using ECondo.Application.Commands.PropertyOccupants.AddToProperty;
using ECondo.Application.Commands.PropertyOccupants.Delete;
using ECondo.Application.Commands.PropertyOccupants.Update;
using ECondo.Application.Data.Occupant;
using ECondo.Application.Queries.PropertyOccupants.GetInProperty;
using ECondo.Application.Queries.PropertyOccupants.GetTenantsInProperty;
using ECondo.Application.Queries.PropertyOccupants.IsUser;
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

    [Authorize]
    [HttpPost(nameof(AcceptInvitation))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> AcceptInvitation(
        [FromBody] AcceptPropertyInvitationCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
    
    [Authorize]
    [HttpGet(nameof(GetInProperty))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(IEnumerable<OccupantResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> GetInProperty(
        [FromQuery] GetOccupantsInPropertyQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(
            data => TypedResults.Json(data),
            CustomResults.Problem);
    }
    
    [Authorize]
    [HttpGet(nameof(GetTenantsInProperty))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(PagedListResponse<OccupantResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IResult> GetTenantsInProperty(
        [FromQuery] GetTenantsInPropertyQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(
            data => TypedResults.Json(data.ToPagedListResponse()),
            CustomResults.Problem);
    }
    
    [Authorize]
    [HttpGet(nameof(IsOccupant))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(UserOccupantResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IResult> IsOccupant(
        [FromQuery] IsUserPropertyOccupantQuery request)
    {
        var result = await sender.Send(request);

        return result.Match(
            data => TypedResults.Json(data),
            CustomResults.Problem);
    }
    
    [Authorize]
    [HttpPut(nameof(Update))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IResult> Update(
        [FromBody] UpdatePropertyOccupantCommand request)
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
    public async Task<IResult> Delete(
        [FromQuery] DeletePropertyOccupantCommand request)
    {
        var result = await sender.Send(request);

        return result.Match(TypedResults.Ok, CustomResults.Problem);
    }
}