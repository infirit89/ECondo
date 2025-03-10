using ECondo.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Extensions;

internal static class IdentityErrorExtension
{
    public static Error[] ToErrorArray(this IEnumerable<IdentityError> errors)
    {
        return errors.Select(identityError => new Error
        {
            Code = identityError.Code,
            Description = identityError.Description,
        }).ToArray();
    }
}