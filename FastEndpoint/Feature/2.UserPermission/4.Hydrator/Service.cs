using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace FastEndpoint.Feature.Service;

sealed class UserPermissionHydrator(UserPermissionService service) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        if (userId == null) return principal;

        var permissions = await service.Get(userId, CancellationToken.None);
        principal.AddIdentity(new(permissions.Select(p => new Claim("permissions", p))));
        return principal;
    }
}