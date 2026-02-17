using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace FastEndpoint.Feature.Service;

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


public static class MemoryCacheDebug
{
    public static IEnumerable<object> Dump(IMemoryCache cache)
    {
        if (cache is not MemoryCache memCache)
            yield break;

        var entriesProp = typeof(MemoryCache).GetProperty(
            "EntriesCollection",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var entries = entriesProp?.GetValue(memCache) as dynamic;
        if (entries == null) yield break;

        foreach (var item in entries)
            yield return item;
    }
}