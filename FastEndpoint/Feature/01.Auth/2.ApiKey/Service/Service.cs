using FastEndpoint;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using FastEndPoint.Feature.Domain;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

public class ApiKeyAuthService(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, AppDbContext db)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(Authentication.ApiKeyHeader, out var apiKey);
        if (await db.Set<ApiKey>().SingleOrDefaultAsync(x => x.Key == apiKey) == null)
        {
            return AuthenticateResult.Fail("Invalid API credentials!");
        }
            
        var identity = new ClaimsIdentity(
            claims: [new Claim("ApiKey", apiKey!)],authenticationType: Scheme.Name);
        var principal = new GenericPrincipal(identity, roles: null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}