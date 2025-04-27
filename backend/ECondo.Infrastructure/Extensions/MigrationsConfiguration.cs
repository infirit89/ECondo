using ECondo.Infrastructure.Contexts;
using ECondo.Infrastructure.Extensions.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Infrastructure.Extensions;

public static class MigrationsConfiguration
{
    public static async Task<IApplicationBuilder> ApplyMigrationsAsync(this IApplicationBuilder applicationBuilder)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ECondoDbContext>();
        await db.Database.MigrateAsync();

        await applicationBuilder.SeedRoles();
        await applicationBuilder.SeedUsers();
        await applicationBuilder.SeedProvinces();
        await applicationBuilder.SeedBuildings();
        await applicationBuilder.SeedEntrances();
        await applicationBuilder.SeedPropertyTypes();
        await applicationBuilder.SeedProperties();
        await applicationBuilder.SeedOccupantTypes();
        await applicationBuilder.SeedOccupants();

        return applicationBuilder;
    }
}
