using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        return services;
    }
}

public static partial class ServiceConfig
{
    public static IServiceCollection AddAuthApiKeyConfig<TAuthApiKeyService>(this IServiceCollection services)
        where TAuthApiKeyService : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddScheme<AuthenticationSchemeOptions, TAuthApiKeyService>(Authentication.ApiKeyScheme, null);

        return services;
    }
}

public static partial class AuthConfig
{
    public readonly static TimeSpan TokenLifetime = Util.Environment.IsProduction() ? TimeSpan.FromMinutes(30) : TimeSpan.FromDays(1);
    public readonly static TimeSpan RefreshTokenLifetime = Util.Environment.IsProduction() ? TimeSpan.FromHours(4) : TimeSpan.FromDays(1);
}

public static class Authentication
{
    public const string JwtBearerScheme = "Bearer";
    public const string ApiKeyScheme = "ApiKey";
    public const string ApiKeyHeader = "x-api-key";
}