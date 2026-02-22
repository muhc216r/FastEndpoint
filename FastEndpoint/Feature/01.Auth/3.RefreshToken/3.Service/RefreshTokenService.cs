using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndpoint.Feature;

[RegisterService<RefreshTokenService>(LifeTime.Scoped)]
public class RefreshTokenService(IMemoryCache cache)
{
    private const string Prefix = "refresh-token:user:";

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
        if(refreshToken != null) cache.Remove(GetKey(userId));

        return refreshToken ;
    }

    private string GetKey(int userId) => $"{Prefix}{userId}";
}
