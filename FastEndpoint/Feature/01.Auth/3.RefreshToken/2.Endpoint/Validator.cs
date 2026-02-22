using FastEndpoint.Feature;

namespace FastEndPoint.Feature.Endpoint;

sealed class AuthRefreshTokenValidator(IHttpContextAccessor httpContext)
    : IPreProcessor<AuthRefreshTokenRequest>
{
    public Task PreProcessAsync(IPreProcessorContext<AuthRefreshTokenRequest> context, CancellationToken cancellation)
    {
        var refreshTokenService = context.Resolve<RefreshTokenService>();
        var userId = httpContext.UserId().GetValueOrDefault();
        var userToken = context?.Request?.Token;

        var refreshToken = refreshTokenService.Get(userId);
        if (refreshToken == null) throw new Exception(MessageResource.NotFound);

        if (!string.Equals(userToken, refreshToken.Token, StringComparison.Ordinal))
        {
            throw new Exception(MessageResource.Invalid);
        }

        if (refreshToken.Expiry < DateTime.UtcNow) throw new Exception(MessageResource.Expired);

        return Task.CompletedTask;
    }
}