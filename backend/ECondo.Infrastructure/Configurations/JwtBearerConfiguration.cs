using ECondo.Domain.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECondo.Infrastructure.Configurations
{
    public class JwtBearerConfiguration(IOptions<JwtSettings> jwtSettings) : IConfigureNamedOptions<JwtBearerOptions>
    {
        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
            };
        }

        public void Configure(string? name, JwtBearerOptions options)
            => Configure(options);

        private readonly JwtSettings _settings = jwtSettings.Value;
    }
}