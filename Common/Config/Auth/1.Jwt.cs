using FastEndpoints.Security;
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

        return services;
    }
}