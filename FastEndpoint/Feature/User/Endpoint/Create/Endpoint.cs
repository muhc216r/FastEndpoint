using FastEndpoint;
using FastEndPoint.Feature.Domain;

namespace FastEndPoint.Feature.Endpoint;
public class UserCreate (AppDbContext db): Endpoint<UserCreateRequest, int>
{
    public override void Configure()
    {
        Post("user");
        //Permissions(UserPermission.Create);
        //Permissions(Permission.Create);
        AllowAnonymous();
        PreProcessor<UserCreateValidator>();
    }

    public override async Task HandleAsync(UserCreateRequest command, CancellationToken cancellation)
    {
        var user = new User(command.FirstName,command.LastName,command.UserName,command.Password);
        db.Set<User>().Add(user);
        await db.SaveChangesAsync(cancellation);
        Response = user.Id;
    }
}