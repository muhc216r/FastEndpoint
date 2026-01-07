using NSwag;
using FastEndpoints.Swagger;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static partial class ServiceConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, bool addApiKey = false)
    {
        if (addApiKey)
        {
            services.SwaggerDocument(x =>
            {
                x.DocumentSettings = y =>
                {
                    y.AddAuth(Authentication.ApiKeyScheme, new()
                    {
                        Name = Authentication.ApiKeyHeader,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Type = OpenApiSecuritySchemeType.ApiKey,
                    });
                };
            });
        }
        else
        {
            services.SwaggerDocument();
        }

        return services;
    }
}