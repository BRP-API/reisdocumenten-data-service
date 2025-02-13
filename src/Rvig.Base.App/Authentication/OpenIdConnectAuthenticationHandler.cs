using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rvig.HaalCentraalApi.Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace Rvig.Base.App.Authentication;

public class OpenIdConnectAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	const string _authorizationHeaderValuePrefix = "Bearer ";
	const string _noClaimsMessage = "No claims in token.";
	private readonly ILoggingHelper _loggingHelper;

	public OpenIdConnectAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
		ILoggingHelper loggingHelper
	) : base(options, logger, encoder, clock)
	{
		_loggingHelper = loggingHelper;
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.ContainsKey("Authorization") || string.IsNullOrWhiteSpace(Request.Headers["Authorization"].ToString()))
		{
			return Task.FromResult(AuthenticateResult.Fail("No token was received from user."));
		}

		var authorizationHeaderValue = Request.Headers["Authorization"].ToString();
		var userClientToken = authorizationHeaderValue?[_authorizationHeaderValuePrefix.Length..].Trim();

		var handler = new JwtSecurityTokenHandler();
		var jsonToken = handler.ReadToken(userClientToken);

		var userClaims = new List<string>();
		if (jsonToken is JwtSecurityToken token)
		{
			userClaims = token.Claims.Where(claim => claim.Type == "claims").Select(x => x.Value).ToList();
		}

		if (userClaims.Count == 0)
		{
			_loggingHelper.LogError(_noClaimsMessage);
			return Task.FromResult(AuthenticateResult.Fail(_noClaimsMessage));
		}

		var claimsPrincipal = CreateClaimsPrincipal(userClaims);
		return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
	}

	private ClaimsPrincipal CreateClaimsPrincipal(List<string> userClaims)
	{
		var claims = new List<Claim>();
		var afnemerId = "";
		foreach (var claim in userClaims)
		{
			if (claim.Contains("OIN=") || claim.Contains("gemeenteCode="))
			{
				claims.Add(new Claim("claims", claim));
			}
			else if (claim.Contains("afnemerID="))
			{
				afnemerId = claim.Replace("afnemerID=", "");
				claims.Add(new Claim("claims", claim));
				claims.Add(new Claim(ClaimTypes.Name, afnemerId));
				claims.Add(new Claim(ClaimTypes.NameIdentifier, afnemerId));
			}
		}
		var authenticatedUser = new GenericIdentity(afnemerId, "OpenIdConnectAuthentication");
		var claimsPrincipal = new ClaimsPrincipal(authenticatedUser);
		ClaimsIdentity claimsIdentity = new();
		claimsIdentity.AddClaims(claims);
		claimsPrincipal.AddIdentity(claimsIdentity);

		_loggingHelper.LogDebug("Successfully validated client token.");

		return claimsPrincipal;
	}
}
