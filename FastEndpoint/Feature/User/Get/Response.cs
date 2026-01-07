using FastEndPoint.Feature.Domain;

namespace FastEndpoint.Feature.Endpoint;
public class UserGetResponse(User user)
{
    public int Id { get; } = user.Id;
    public string? FirstName { get;} = user.FirstName;
    public string? LastName { get; } = user.LastName;
    public string UserName { get;} = user.UserName;
}
