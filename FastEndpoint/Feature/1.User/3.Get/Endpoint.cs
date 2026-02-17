using FastEndpoint.Feature.Service;
using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndpoint.Feature.Endpoint;
public class UserGet(AppDbContext db,IMemoryCache cache) : EndpointWithoutRequest<UserGetResponse>
{
    public override void Configure()
    {
        Get("user/{id}");
        Permissions(GetType().Name);
    }

    public override async Task HandleAsync(CancellationToken cancellation)
    {
        var all = MemoryCacheDebug.Dump(cache).ToList();

        int id = Route<int>("id");
        var user = await db.Set<User>().SingleAsync(x=>x.Id==id,cancellation);
        Response = new UserGetResponse(user!);
    }
}