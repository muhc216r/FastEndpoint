using System.Collections.Concurrent;

namespace IdentityServer.Feature.Endpoint;
public class CreateOrUpdateUserPermission(AppDbContext db) : Endpoint<CreateOrUpdateUserPermissionRequest>
{
    public static readonly ConcurrentDictionary<int, StoreRefreshToken> RefreshTokens = new();
    public override void Configure()
    {
        Post("auth/user-permission");
        Permissions(GetType().Name);
    }

    public override async Task HandleAsync(CreateOrUpdateUserPermissionRequest command,CancellationToken cancellation)
    {
        _=await db.Set<UserPermission>().ExecuteDeleteAsync(cancellation);

        var userPermission =command.Permissions.Select(x=> new UserPermission(command.UserId,x)).ToArray();
        db.AddRange(userPermission);
        await db.SaveChangesAsync(cancellation);
    }
}
