global using Common;
global using Common.Extension;
global using FastEndpoints;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NSwag;
using FastEndpoint;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddDbConfig<AppDbContext>(AppConfig.Connection)
    .AddAuthConfig(AppConfig.SigningKey, AppConfig.Issuer, AppConfig.Audience)
    .AddFastEndpoints()
    //.AddJobQueues<JobRecord, JobStorageProvider>()
    .SwaggerDocument(x =>
    {
        x.DocumentSettings = y =>
        {
            y.AddAuth(ApiKeyAuth.SchemeName, new()
            {
                Name = ApiKeyAuth.HeaderName,
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.ApiKey,
            });
        };
    });

var app = builder.Build();
app.UseAuthentication().UseAuthorization()
   .UseDefaultExceptionHandler(useGenericReason: !app.Environment.IsDevelopment())
   .UseFastEndpoints(x => { x.Endpoints.RoutePrefix = "api"; })
   //.UseJobQueues()
   .UseSwaggerGen();
app.Run();
