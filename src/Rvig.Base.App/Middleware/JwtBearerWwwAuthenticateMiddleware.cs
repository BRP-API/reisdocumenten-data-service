using Microsoft.AspNetCore.Http;
using System.Net;

namespace Rvig.Base.App.Middleware;

public class JwtBearerWwwAuthenticateMiddleware
{
	private readonly RequestDelegate _next;

	public JwtBearerWwwAuthenticateMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized && context.Response.Headers.Any(header => header.Key.Equals("WWW-Authenticate")))
		{
			// Replace header
			context.Response.Headers.Remove("WWW-Authenticate");
			context.Response.Headers.Add("WWW-Authenticate", "x-invalid_token");
		}

		await _next(context);
	}
}
