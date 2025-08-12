using ECondo.SharedKernel.Result;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Extensions;

internal static class IdentityErrorExtension
{
    public static ValidationError ToValidationError(this IEnumerable<IdentityError> errors)
    {
        return new ValidationError(
            errors
                .Select(identityError => 
                    Error.Problem(identityError.Code, identityError.Description))
                .ToArray());
    }
}