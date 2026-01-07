using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static partial class ServiceConfig
{
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, string signingKey, string issuer, string audience)
    {

        services
            .Configure<JwtCreationOptions>(x =>
            {
                x.SigningKey = signingKey;
                x.ExpireAt = DateTime.UtcNow.Add(AuthConfig.TokenLifetime);
                x.Issuer = issuer;
                x.Audience = audience;
            })
            .Configure<JwtSigningOptions>(x => { x.SigningKey = signingKey; })
            .AddAuthenticationJwtBearer(x => { },
                x =>
                {
                    x.TokenValidationParameters.ValidIssuer = issuer;
                    x.TokenValidationParameters.ValidAudience = audience;
                })
            .AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddScheme<AuthenticationSchemeOptions, ApiKeyAuth>(ApiKeyAuth.SchemeName, null);

        return services;
    }
}

public static partial class AuthConfig
{
    public readonly static TimeSpan TokenLifetime = Util.Environment.IsProduction() ? TimeSpan.FromMinutes(30) : TimeSpan.FromDays(1);
    public readonly static TimeSpan RefreshTokenLifetime = Util.Environment.IsProduction() ? TimeSpan.FromHours(4) : TimeSpan.FromDays(1);
}

public static class AuthenticationScheme
{
    public const string JwtBearer = "Bearer";
    public const string ApiKey = "ApiKey";
}