using ECondo.Application.Extensions;
using ECondo.Infrastructure.Extensions;
using ECondo.Api.Extensions;

namespace ECondo.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPresentation();

            WebApplication app = builder.Build();

            await app.ApplyMigrationsAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            app.UseExceptionHandler();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            await app.RunAsync();
        }
    }
}
