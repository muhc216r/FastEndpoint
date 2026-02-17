using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndpoint.Feature.Service;

[RegisterService<UserPermissionService>(LifeTime.Scoped)]
public class UserPermissionService(IMemoryCache cache, AppDbContext db)
{
    private static string Key(string userId) => $"user:{userId}:permissions";
    private static readonly TimeSpan SlidingTtl = TimeSpan.FromHours(1);
    private static readonly TimeSpan AbsoluteTtl = TimeSpan.FromHours(10);

    public async Task<string[]> Get(string userId, CancellationToken cancellation)
    {
        var key = Key(userId);
        var permissions = await cache.GetOrCreateAsync(key, x => CreatePermissionsCacheEntry(x, cancellation));
        return permissions!;
    }

    private async Task<string[]> CreatePermissionsCacheEntry(ICacheEntry entry, CancellationToken cancellation)
    {
        entry.SlidingExpiration = SlidingTtl;
        entry.Priority = CacheItemPriority.High;
        entry.AbsoluteExpirationRelativeToNow = AbsoluteTtl;

        var permissions = await db.Set<Permission>().Select(x => x.Name).ToArrayAsync(cancellation);
        return permissions ?? [];
    }

    public void Invalidate(string userId) => cache.Remove(Key(userId));
}