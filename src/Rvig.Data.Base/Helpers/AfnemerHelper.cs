using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rvig.Data.Base.Services;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Helpers
{
	public static class AfnemerHelper
	{
		public static Afnemer GetAfnemerInfoFromAuthenticatedUser(IHttpContextAccessor httpContextAccessor)
		{
			int afnemerCode;
			short gemeenteCode = 0;
			if (AppSettingsManager.Configuration?.GetValue<string>("AuthenticationTypes")?.ToLower()?.Equals("basic") == true)
			{
				var username = httpContextAccessor.HttpContext?.User.Identity?.Name;
				if (username == null)
					throw new AuthenticationException("User naam van geauthenticeerde afnemer niet aanwezig op http context.");

				if (!username.Contains('|'))
				{
					if (!int.TryParse(username, out afnemerCode))
						throw new AuthenticationException("Integer verwacht voor afnemercode.");

					return new Afnemer { Afnemerscode = afnemerCode };
				}

				var split = username.Split('|', 2);

				if (!int.TryParse(split[0], out afnemerCode))
				{
					throw new AuthenticationException("Integer verwacht voor afnemercode.");
				}

				if (!string.IsNullOrWhiteSpace(split[1]) && !short.TryParse(split[1], out gemeenteCode))
				{
					throw new AuthenticationException("Integer verwacht voor gemeenteCode.");
				}
				else if (string.IsNullOrWhiteSpace(split[1]))
				{
					return new Afnemer { Afnemerscode = afnemerCode };
				}
			}
			else if (AppSettingsManager.Configuration?.GetValue<string>("AuthenticationTypes")?.ToLower()?.Equals("jwtbearer") == true
				|| AppSettingsManager.Configuration?.GetValue<string>("AuthenticationTypes")?.ToLower()?.Equals("openidconnect") == true)
			{
				if (!int.TryParse(httpContextAccessor.HttpContext?.User.Identity?.Name, out afnemerCode))
				{
					throw new AuthenticationException("Integer verwacht voor afnemercode.");
				}

				var gemeenteCodeClaim = httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(claim => claim.Value.Contains("gemeenteCode"));

				// Explicit null checks is because there is different logic for the lack of a gemeenteCode.
				// This check will validate that if there is a value then it has to be an integer.
				if (gemeenteCodeClaim != null && !string.IsNullOrWhiteSpace(gemeenteCodeClaim.Value)
					&& !short.TryParse(gemeenteCodeClaim?.Value?.Replace("gemeenteCode=", ""), out gemeenteCode))
				{
					throw new AuthenticationException("Integer verwacht voor gemeenteCode.");
				}
				else if (gemeenteCodeClaim == null || string.IsNullOrWhiteSpace(gemeenteCodeClaim.Value))
				{
					return new Afnemer { Afnemerscode = afnemerCode };
				}
			}
			else
			{
				throw new AuthenticationException("Invalid authentication method.");
			}

			return new Afnemer { Afnemerscode = afnemerCode, Gemeentecode = gemeenteCode };
		}
	}
}