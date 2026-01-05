using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Common.Extension;
public static class IHttpContextAccessorExtension
{
    public static int? UserId(this IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;
        var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userId, out var id) ? id : null;
    }
}
