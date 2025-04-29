using ECondo.Api.Extensions;
using ECondo.Application.Queries.PropertyTypes.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyTypeController(ISender sender) : ControllerBase
{
    [HttpGet(nameof(GetAll))]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(PropertyTypeNameResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult> GetAll()
    {
        var result = await sender
            .Send(new GetAllPropertyTypesQuery());

        return result.Match(
            data => TypedResults.Json(data),
            CustomResults.Problem);
    }
}