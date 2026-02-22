using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static partial class ServiceConfig
{
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, string issuer, string audience)
    {
        services
            .Configure<JwtCreationOptions>(x =>
            {
                x.SigningKey = File.ReadAllText("Common/jwt-private-key.pem");
                x.SigningStyle = TokenSigningStyle.Asymmetric;
                x.SigningAlgorithm = SecurityAlgorithms.RsaSha256;
                x.ExpireAt = DateTime.UtcNow.Add(AuthConfig.TokenLifetime);
                x.Issuer = issuer;
                x.Audience = audience;
                x.KeyIsPemEncoded = true;
            })
            .Configure<JwtSigningOptions>(x =>
            {
                x.SigningKey = File.ReadAllText("Common/jwt-public-key.pem");
                x.KeyIsPemEncoded = true;
                x.SigningStyle = TokenSigningStyle.Asymmetric;
            });

        services.AddAuthenticationJwtBearer(x => { },
            y =>
            {
                y.TokenValidationParameters.ValidIssuer = issuer;
                y.TokenValidationParameters.ValidAudience = audience;
            })

             .AddAuthorization();

        return services;
    }
}