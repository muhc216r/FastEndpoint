namespace FastEndPoint.Feature.Endpoint;
sealed class UserCreateValidator : IPreProcessor<UserCreateRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<UserCreateRequest> context, CancellationToken cancellation)
    {
        //var db= context.Resolve<AppDbContext>();
        //var users =await db.User.ToArrayAsync(cancellation);
    }
}