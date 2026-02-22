using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndpoint.Feature;

[RegisterService<RefreshTokenService>(LifeTime.Scoped)]
public class RefreshTokenService(IMemoryCache cache)
{
    public void Add(StoreRefreshToken refreshToken)
    {
        cache.Set(
            key: GetKey(refreshToken.UserId),
            value: refreshToken,
            options: new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
    }

    public StoreRefreshToken? Get(int userId)
    {
        cache.TryGetValue(GetKey(userId), out StoreRefreshToken? refreshToken);
        return refreshToken;
    }

    public void Remove(int userId) => cache.Remove(GetKey(userId));

    private string GetKey(int userId) => $"refresh-token:user:{userId}";
}
