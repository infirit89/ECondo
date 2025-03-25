using ECondo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Infrastructure.Extensions;

public static class MigrationsConfiguration
{
    public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder applicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ECondoDbContext>();
        await db.Database.MigrateAsync();

        await applicationBuilder.SeedUsers();
        await applicationBuilder.SeedProvinces();

        return applicationBuilder;
    }
}
