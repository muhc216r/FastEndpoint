using FastEndpoint;
using FastEndpoints.Security;
using System.Security.Claims;
using FastEndPoint.Feature.Domain;

namespace FastEndPoint.Feature.Endpoint;
public class AuthLogin(AppDbContext db) : Endpoint<AuthLoginRequest, AuthLoginResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
        PreProcessor<AuthLoginValidator>();
    }

    public override async Task HandleAsync(AuthLoginRequest command, CancellationToken cancellation)
    {
        var user = await db.Set<User>().SingleAsync(x => x.UserName == command.Username, cancellation);

        var permissions = await db.Set<Permission>().Select(x => x.Name).ToArrayAsync(cancellation);
        //var permissions=await db.Set<UserPermission>().Where(x=>x.UserId==userId).Select(x=>x.Permission).ToArrayAsync(cancellation);

        var token = JwtBearer.CreateToken(x =>
        {
            x.SigningKey = File.ReadAllText("Common/jwt-private-key.pem");
            x.User.Claims.Add(new Claim("UserId", user.Id.ToString()));
            x.User.Permissions.AddRange(permissions);
            //x.User.Roles.Add("Role1", "Role2");
        });

        var refreshToken = new StoreRefreshToken(user.Id);
        RefreshTokens.AddOrUpdate(refreshToken.UserId, refreshToken, (_, __) => refreshToken);
        await Send.OkAsync(new AuthLoginResponse(token, refreshToken.Token), cancellation);
    }
}
