global using Common;
global using FastEndpoints;
global using Common.Extension;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

using FastEndpoint;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddDbConfig<AppDbContext>(AppConfig.Connection)
    .AddAuthConfig(AppConfig.Issuer, AppConfig.Audience)
    .AddAuthApiKeyConfig<ApiKeyAuthService>()
    .AddFastEndpoints()
    .AddSwaggerConfig(true);
//.AddJobQueues<JobRecord, JobStorageProvider>()


var app = builder.Build();
app.UseAuthentication().UseAuthorization()
   .UseDefaultExceptionHandler(useGenericReason: !app.Environment.IsDevelopment())
   .UseFastEndpoints(x => { x.Endpoints.RoutePrefix = "api"; })
   //.UseJobQueues()
   .UseSwaggerGen();
app.Run();
