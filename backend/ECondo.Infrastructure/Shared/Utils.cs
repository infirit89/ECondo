using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Shared;

internal static class Utils
{
    public static void LogIdentityErrors<T>(this ILogger<T> logger, IEnumerable<IdentityError>  errors)
    {
        foreach (var error in errors)
            logger.LogError($"Code: '{error.Code}'; Description: '{error.Description}'");
    }
}