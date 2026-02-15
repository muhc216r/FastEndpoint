using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Common;
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