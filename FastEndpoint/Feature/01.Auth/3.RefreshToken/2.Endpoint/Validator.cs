using Microsoft.Extensions.Caching.Memory;

namespace FastEndPoint.Feature.Endpoint;

sealed class AuthRefreshTokenValidator(IHttpContextAccessor httpContext, IMemoryCache cache)
    : IPreProcessor<AuthRefreshTokenRequest>
{
    public Task PreProcessAsync(IPreProcessorContext<AuthRefreshTokenRequest> context, CancellationToken cancellation)
    {
        var userId = httpContext.UserId().GetValueOrDefault();

        var exists = AuthLogin.RefreshTokens.TryGetValue(userId, out var storedToken);
        if (!exists) throw new Exception(MessageResource.NotFound);

        if (!string.Equals(context!.Request!.Token, storedToken!.Token, StringComparison.Ordinal))
        {
            throw new Exception(MessageResource.Invalid);
        }

        if (storedToken.Expiry < DateTime.UtcNow) throw new Exception(MessageResource.Expired);

        return Task.CompletedTask;
    }
}