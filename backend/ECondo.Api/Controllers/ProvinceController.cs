using ECondo.Api.Extensions;
using ECondo.Application.Queries.Province;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<Results<JsonHttpResult<ProvinceNameResult>, ValidationProblem>>
        GetProvinces()
    {
        var result = await sender.Send(new GetProvincesQuery());
        return result switch
        {
            Result<ProvinceNameResult, Error>.Success s =>
                TypedResults.Json(s.Data),

            Result<ProvinceNameResult, Error>.Error e =>
                e.Data.ToValidationProblem(),

            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
    }
}