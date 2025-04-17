using ECondo.Api.Extensions;
using ECondo.Application.Queries.Provinces.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvinceController(ISender sender) : ControllerBase
{
    [HttpGet(nameof(GetProvinces))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,
        Type = typeof(HttpValidationProblemDetails))]
    public async Task<IResult> GetProvinces()
    {
        var result = await sender.Send(new GetProvincesQuery());

        return result.Match(data => TypedResults.Json(data), CustomResults.Problem);
    }
}