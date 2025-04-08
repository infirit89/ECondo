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
