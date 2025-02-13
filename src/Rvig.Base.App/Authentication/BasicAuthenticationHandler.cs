using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rvig.BrpApi.Shared.Exceptions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;

namespace Rvig.Base.App.Authentication;
/// <summary>
/// Temporary authentication handler for test purposes, should be updated with the correct authentication (OAuth?).
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        const string prefix = "Basic ";
        var authorizationValue = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationValue) || !authorizationValue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var credentialsBase64 = authorizationValue.Substring(prefix.Length).Trim();
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentialsBase64));
        var credentialsSplitted = credentials.Split(":", 2);
        var authUsername = credentialsSplitted[0];
        var authPassword = credentialsSplitted.Length > 1 ? credentialsSplitted[1] : throw new AuthenticationException("Unable to get password from basic authentication credentials");

        if (authPassword != "tempsolution!")
        {
            return Task.FromResult(AuthenticateResult.Fail(""));
        }

        var authenticatedUser = new GenericIdentity(authUsername, "BasicAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(authenticatedUser);

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
    }
}
