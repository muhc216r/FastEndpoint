namespace IdentityServer.Feature.Endpoint;
public record CreateOrUpdateUserPermissionRequest(int UserId,string[] Permissions);