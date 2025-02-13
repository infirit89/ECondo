using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ECondo.Infrastructure.Extensions;

internal static class IdentityBuilderExtension
{
    public static IdentityBuilder AddSecurityStampValidation(this IdentityBuilder identityBuilder)
    {
        identityBuilder.Services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
        return identityBuilder;
    }
}
