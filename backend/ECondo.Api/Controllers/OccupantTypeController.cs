using ECondo.Api.Extensions;
using ECondo.Application.Queries.OccupantTypes.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OccupantTypeController(ISender sender) : ControllerBase
{
    [HttpGet(nameof(GetAll))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(OccupantTypeNameResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult> GetAll()
    {
        var result = await sender
            .Send(new GetAllOccupantTypesQuery());

        return result.Match(
            data => TypedResults.Json(data),
            CustomResults.Problem);
    }
}
