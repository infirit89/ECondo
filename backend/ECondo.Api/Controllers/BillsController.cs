using ECondo.Api.Extensions;
using ECondo.Application.Commands.Payment.CreateBill;
using ECondo.Application.Commands.Properties.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IResult> Create(
            [FromBody] CreateBillCommand request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(new { Id = data }), 
                CustomResults.Problem);
        }
    }
}
