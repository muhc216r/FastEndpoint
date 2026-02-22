using FastEndpoint;
using System.Security.Claims;
using FastEndpoints.Security;
using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace FastEndPoint.Feature.Endpoint;
public class AuthRefreshToken(IHttpContextAccessor httpContext,AppDbContext db, IMemoryCache cache) 
    : Endpoint<AuthRefreshTokenRequest, AuthLoginResponse>
{
    
    public override void Configure()
    {
        Post("auth/refresh-token");
        PreProcessor<AuthRefreshTokenRequest>();
    }

    public override async Task HandleAsync(AuthRefreshTokenRequest request, CancellationToken cancellation)
    {
        var passIsValid = true;
        var userId= httpContext.UserId().HasValue ? httpContext.UserId()!.Value:1;
        if (passIsValid)
        {
            var permissions = await db.Set<Permission>().Select(x => x.Name).ToArrayAsync(cancellation);
            var token = JwtBearer.CreateToken(async x =>
            {
                x.SigningKey = File.ReadAllText("Common/jwt-private-key.pem");
                x.User.Claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

                x.User.Permissions.AddRange(permissions);
                /*
                    u.Roles.Add("Admin");
                    u.Permissions.AddRange(new[] { "Create_Item", "Delete_Item" });
                    u.Claims.Add(new("Address", "123 Street"));

                    //indexer based claim setting
                    u["Email"] = "abc@def.com";
                    u["Department"] = "Administration";
                 */
            });

            var refreshToken = new StoreRefreshToken(userId);
            var cacheKey = $"{RefreshKeyPrefix}{userId}";
            cache.Set(
                key: cacheKey,
                value: refreshToken,
                options: new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                    // SlidingExpiration = TimeSpan.FromDays(2)
                });
            Response = new AuthLoginResponse(token, refreshToken.Token);
        }
    }
}