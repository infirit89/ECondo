using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ECondo.Infrastructure.Shared;

namespace ECondo.Infrastructure.Extensions.Seeders;

internal class UserSeeder
{
}

internal static class UserSeederConfiguration
{
    public static async Task<IApplicationBuilder> SeedUsers(this IApplicationBuilder appBuilder)
    {
        await using var scope = appBuilder.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var sender = services.GetRequiredService<ISender>();
        var logger = services.GetRequiredService<ILogger<UserSeeder>>();

        void PrintIdentityErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
                logger.LogError(error.Description);
        }


        var basicUserRes = await sender.Send(UserSeedData.BasicUser);
        if (!basicUserRes.IsOk())
            PrintIdentityErrors(basicUserRes.ToError().Data!);

        var basicUserProfileRes = await sender.Send(UserSeedData.BasicUserProfile);

        if (!basicUserProfileRes.IsOk())
            logger.LogError(basicUserProfileRes.ToError().Data!.Description);

        return appBuilder;
    }
}
