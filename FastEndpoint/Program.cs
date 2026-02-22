global using Common;
global using FastEndpoints;
global using Common.Extension;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

using FastEndpoint;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddFastEndpoints()
    .AddSwaggerConfig(true)
    .AddHttpContextAccessor()
    .AddAuthApiKeyConfig<ApiKeyAuthService>()
    .AddDbConfig<AppDbContext>(AppConfig.Connection)
    .AddAuthConfig(AppConfig.Issuer, AppConfig.Audience);
//.AddJobQueues<JobRecord, JobStorageProvider>()


var app = builder.Build();
app.UseAuthentication().UseAuthorization()
   .UseDefaultExceptionHandler(useGenericReason: !app.Environment.IsDevelopment())
   .UseFastEndpoints(x => { x.Endpoints.RoutePrefix = "api"; })
   //.UseJobQueues()
   .UseSwaggerGen();
app.Run();
