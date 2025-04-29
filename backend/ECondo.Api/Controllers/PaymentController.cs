using ECondo.Api.Extensions;
using ECondo.Application.Commands.Payment.CreateIntent;
using ECondo.Application.Queries.Payment.GetById;
using ECondo.Application.Queries.Payment.GetForProperty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECondo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost(nameof(CreateIntent))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IResult> CreateIntent(
            [FromBody] CreatePaymentIntentCommand request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(new { ClientSecret = data }), 
                CustomResults.Problem);
        }
        
        [Authorize]
        [HttpGet(nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IResult> GetById(
            [FromQuery] GetPaymentByIdQuery request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(data), 
                CustomResults.Problem);
        }
        
        [Authorize]
        [HttpGet(nameof(GetForProperty))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(HttpValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IResult> GetForProperty(
            [FromQuery] GetPaymentForPropertyQuery request)
        {
            var result = await sender.Send(request);

            return result.Match(
                data => TypedResults.Json(data.ToPagedListResponse()), 
                CustomResults.Problem);
        }
    }
}
