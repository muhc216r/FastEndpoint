using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

public class ApiKeyAuthService(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string HeaderName = "x-api-key";
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(HeaderName, out var apiKey);

        if (false)
            return Task.FromResult(AuthenticateResult.Fail("Invalid API credentials!"));

        var identity = new ClaimsIdentity(
            claims: [new Claim("ClientID", "Default")],
            authenticationType: Scheme.Name);
        var principal = new GenericPrincipal(identity, roles: null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}