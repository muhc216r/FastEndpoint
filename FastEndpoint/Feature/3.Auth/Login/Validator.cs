using FastEndpoint;
using FastEndPoint.Feature.Domain;

namespace FastEndPoint.Feature.Endpoint;
sealed class AuthLoginValidator : IPreProcessor<AuthLoginRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<AuthLoginRequest> context, CancellationToken cancellation)
    {
        //var logger = context.HttpContext.Resolve<ILogger<AuthLoginRequest>>();
        //logger.LogInformation($"request:{context?.Request?.GetType().FullName} path: {context?.HttpContext.Request.Path}");
        //return Task.CompletedTask;

        var db = context.Resolve<AppDbContext>();
        var user = await db.Set<User>().SingleOrDefaultAsync(x => x.UserName == context.Request!.Username, cancellation);
        var verifyPassword = user?.VerifyPassword(context.Request!.Password);

        if (!verifyPassword.HasValue || !verifyPassword.Value)
        {
            await context.SendErrors(MessageResource.InvalidUsernameOrPassword, cancellation);
        }
    }
}