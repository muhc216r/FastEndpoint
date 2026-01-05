using FastEndpoints.Security;
using System.Security.Claims;

namespace IdentityServer.Feature.Endpoint;
public class AuthRefreshToken(IHttpContextAccessor httpContext,AppDbContext db) : Endpoint<AuthRefreshTokenRequest, AuthLoginResponse>
{
    public override void Configure()
    {
        Post("auth/refresh-token");
        //PreProcessor<AuthRefreshTokenRequest>();
    }

    public override async Task HandleAsync(AuthRefreshTokenRequest request, CancellationToken cancellation)
    {
        var passIsValid = true;
        var userId=httpContext.UserId()!.Value;
        if (passIsValid)
        {
            var token = JwtBearer.CreateToken(async x => {
                x.User.Claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

                var permissions=await db.Set<Permission>().Select(x=>x.Name).ToArrayAsync();
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
            AuthLogin.RefreshTokens.AddOrUpdate(refreshToken.UserId, refreshToken, (_, __) => refreshToken);
            Response = new AuthLoginResponse(token, refreshToken.Token);
        }
    }
}