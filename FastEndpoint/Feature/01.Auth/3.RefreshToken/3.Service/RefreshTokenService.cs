using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndpoint.Feature;

public class RefreshTokenService(IMemoryCache cache)
{
    private const string Prefix = "refresh-token:user:";
    private static readonly TimeSpan RefreshTokenSlidingExpiration = TimeSpan.FromHours(24);
    private static readonly TimeSpan RefreshTokenAbsoluteExpiration = TimeSpan.FromHours(24);


    public void Save(string userId, StoreRefreshToken refreshToken)
    {
        var cacheKey = $"{Prefix}{userId}";
        cache.Set(
            key: cacheKey,
            value: refreshToken,
            options: new MemoryCacheEntryOptions
            {
                SlidingExpiration = RefreshTokenSlidingExpiration,
                AbsoluteExpirationRelativeToNow = RefreshTokenAbsoluteExpiration
            });
    }

    public bool TryGetRefreshToken(string userId, out StoreRefreshToken? refreshToken)
    {
        var cacheKey = $"{Prefix}{userId}";
        return cache.TryGetValue(cacheKey, out refreshToken);
    }

    public void RemoveRefreshToken(string userId)
    {
        var cacheKey = $"{Prefix}{userId}";
        cache.Remove(cacheKey);
    }
}
