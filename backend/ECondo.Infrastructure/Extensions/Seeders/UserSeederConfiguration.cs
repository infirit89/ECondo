using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
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

        void PrintIdentityErrors(Error error)
        {
            if (error is ValidationError validationError)
            {
                foreach (var err in validationError.Errors)
                    logger.LogError(err.ToString());
                return;
            }

            logger.LogError(error.ToString());
        }


        foreach (var userData in UserSeedData.Users)
        {
            var userRes = await sender.Send(userData);
            
            if (!userRes.IsOk())
                PrintIdentityErrors(userRes.ToError().Data!);
        }

        foreach (var profileData in UserSeedData.Profiles)
        {
            var userProfileRes = await sender.Send(profileData);

            if (!userProfileRes.IsOk())
                logger.LogError(userProfileRes.ToError().Data!.Description);
        }

        return appBuilder;
    }
}
