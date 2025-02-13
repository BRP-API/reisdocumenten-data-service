using Microsoft.AspNetCore.Http;

namespace Rvig.Base.App.Middleware
{
	public class ForceAcceptAndContentTypeHeadersWithValueMiddleware
	{
		private readonly RequestDelegate _next;

		public ForceAcceptAndContentTypeHeadersWithValueMiddleware(RequestDelegate next) =>
			_next = next;

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Headers.ContainsKey("Accept") && string.IsNullOrWhiteSpace(context.Request.Headers["Accept"]))
			{
				context.Request.Headers["Accept"] = "application/json; charset=utf-8";
			}
			else if (!context.Request.Headers.ContainsKey("Accept"))
			{
				context.Request.Headers.Add("Accept", "application/json; charset=utf-8");
			}
			if (string.IsNullOrWhiteSpace(context.Request.ContentType))
			{
				context.Request.ContentType = "application/json; charset=utf-8";
			}

			await _next(context);
		}
	}
}
