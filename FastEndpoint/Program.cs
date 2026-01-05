global using Common;
global using FastEndpoints;
global using BaseApi;
global using Common.Extension;
global using BaseApi.Feature.Domain;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

using BaseApi.Feature.Job;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddDbConfig<AppDbContext>(AppConfig.Connection)
    .AddAuthConfig(AppConfig.SigningKey, AppConfig.Issuer, AppConfig.Audience)
    .AddFastEndpoints()
    //.AddJobQueues<JobRecord, JobStorageProvider>()
    .SwaggerDocument();

var app = builder.Build();
app.UseAuthentication().UseAuthorization()
   .UseDefaultExceptionHandler(useGenericReason: !app.Environment.IsDevelopment())
   .UseFastEndpoints(x => { x.Endpoints.RoutePrefix = "api"; })
   //.UseJobQueues()
   .UseSwaggerGen();
app.Run();
