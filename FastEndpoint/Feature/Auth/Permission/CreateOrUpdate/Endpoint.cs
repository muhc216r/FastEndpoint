using System.Collections.Concurrent;

namespace IdentityServer.Feature.Endpoint;
public class AuthPermission(AppDbContext db) : EndpointWithoutRequest
{
    public static readonly ConcurrentDictionary<int, StoreRefreshToken> RefreshTokens = new();
    public override void Configure()
    {
        Get("auth/permission");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellation)
    {
        _ = await db.Set<Permission>().ExecuteDeleteAsync(cancellation);

        var endpoints = EndpointScanner.GetAllEndpointTypes().ToArray();
        var permissions = endpoints.Select(x => new Permission(x.Name)).ToList();
        db.AddRange(permissions);
        await db.SaveChangesAsync(cancellation);
    }
}


public class AuthPermissionCommand : ICommand{}
public class AuthPermissionCommandHandler(AppDbContext db) : ICommandHandler<AuthPermissionCommand>
{
    public async Task ExecuteAsync(AuthPermissionCommand command, CancellationToken cancellation)
    {
        _ = await db.Set<Permission>().ExecuteDeleteAsync(cancellation);

        var endpoints = EndpointScanner.GetAllEndpointTypes().ToArray();
        var permissions = endpoints.Select(x => new Permission(x.Name)).ToList();
        db.AddRange(permissions);
        await db.SaveChangesAsync(cancellation);
    }
}

public class NightlyService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            _ = await new AuthPermissionCommand().QueueJobAsync(ct: cancellation);
            await Task.Delay(TimeSpan.FromHours(24), cancellation);
        }
    }
}
