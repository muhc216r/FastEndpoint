using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static partial class ServiceConfig
{
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, string publicKey, string issuer, string audience)
    {
        services
            .Configure<JwtCreationOptions>(x =>
            {
                x.SigningStyle = TokenSigningStyle.Asymmetric;
                x.SigningAlgorithm = SecurityAlgorithms.RsaSha256;
                x.ExpireAt = DateTime.UtcNow.Add(AuthConfig.TokenLifetime);
                x.Issuer = issuer;
                x.Audience = audience;
            });
            //.Configure<JwtSigningOptions>(x => { x.SigningKey = signingKey; });

        services.AddAuthenticationJwtBearer(
            x =>
            {
                x.SigningKey = publicKey;
                x.SigningStyle = TokenSigningStyle.Asymmetric;
                x.KeyIsPemEncoded = true;
            },
           y =>
           {
               y.TokenValidationParameters.ValidIssuer = issuer;
               y.TokenValidationParameters.ValidAudience = audience;
           }).AddAuthorization();

        return services;
    }
}