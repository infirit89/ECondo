using ECondo.Domain.Shared;

namespace ECondo.Api.Extensions;

public static class CustomResults
{
    public static IResult Problem(Error error)
    {
        var title = error.Type switch
        {
            ErrorType.Validation => error.Code,
            ErrorType.Problem => error.Code,
            ErrorType.NotFound => error.Code,
            ErrorType.Conflict => error.Code,
            _ => "Server failure"
        };

        var detail = error.Type switch
        {
            ErrorType.Validation => error.Description,
            ErrorType.Problem => error.Description,
            ErrorType.NotFound => error.Description,
            ErrorType.Conflict => error.Description,
            _ => "An unexpected error occurred"
        };

        var type = error.Type switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            title: title,
            detail: detail,
            type: type,
            statusCode: statusCode,
            extensions: GetErrors(error));

        static Dictionary<string, object?>? GetErrors(Error error)
        {
            if (error is not ValidationError validationError)
            {
                return null;
            }

            var errorDictionary = validationError.Errors.ToErrorDictionary();

            return new Dictionary<string, object?>
            {
                { "errors", errorDictionary }
            };
        }
    }
}