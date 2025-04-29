using ECondo.Api.Extensions;
using ECondo.Application.Commands.Payment.ConnectStripeAccount;
using ECondo.Application.Queries.Payment.CheckStripeStatus;
using ECondo.Application.Queries.Payment.GetStripeLoginLink;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost(nameof(Connect))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IResult> Connect(
            [FromBody] ConnectStripeAccountCommand request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(new { Url = data }), 
                CustomResults.Problem);
        }
        
        [Authorize]
        [HttpGet(nameof(CheckEntranceStatus))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IResult> CheckEntranceStatus(
            [FromQuery] CheckEntranceStripeStatusQuery request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(data), 
                CustomResults.Problem);
        }
        
        [Authorize]
        [HttpGet(nameof(GetLoginLink))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IResult> GetLoginLink(
            [FromQuery] GetEntranceStripeLoginLinkQuery request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(new { Url = data }), 
                CustomResults.Problem);
        }
    }
}
