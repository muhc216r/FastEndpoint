using FastEndPoint.Feature.Domain;

namespace FastEndpoint.Feature.Endpoint;
public class UserGet(AppDbContext db) : EndpointWithoutRequest<UserGetResponse>
{
    public override void Configure()
    {
        Get("user/{id}");
        //Permissions(GetType().Name);
    }

    public override async Task HandleAsync(CancellationToken cancellation)
    {
        int id = Route<int>("id");
        var user = await db.Set<User>().SingleAsync(x=>x.Id==id,cancellation);
        Response = new UserGetResponse(user!);
    }
}