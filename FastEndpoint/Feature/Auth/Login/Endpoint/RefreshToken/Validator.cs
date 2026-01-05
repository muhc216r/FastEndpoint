namespace IdentityServer.Feature.Endpoint;
sealed class AuthRefreshTokenValidator(IHttpContextAccessor httpContext): IPreProcessor<AuthRefreshTokenRequest>
{
    public Task PreProcessAsync(IPreProcessorContext<AuthRefreshTokenRequest> context, CancellationToken cancellation)
    {
        var userId = httpContext.UserId().GetValueOrDefault();

        var exists = AuthLogin.RefreshTokens.TryGetValue(userId, out var storedToken);
        if (!exists)throw new Exception("refresh token not found!");

        if (!string.Equals(context!.Request!.Token, storedToken!.Token, StringComparison.Ordinal))
        {
            throw new Exception("refresh token is invalid!");
        }

        if (storedToken.Expiry < DateTime.UtcNow) throw new Exception("refresh token has expired!");

        return Task.CompletedTask;
    }
}