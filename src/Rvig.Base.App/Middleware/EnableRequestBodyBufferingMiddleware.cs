using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using System.IO;
using System.Text;

namespace Rvig.Base.App.Middleware
{
	public class EnableRequestBodyBufferingMiddleware
	{
		private readonly RequestDelegate _next;

		public EnableRequestBodyBufferingMiddleware(RequestDelegate next) =>
			_next = next;

		public async Task InvokeAsync(HttpContext context)
		{
			context.Request.EnableBuffering();

			if (!context.Items.ContainsKey("RequestBodySerialized"))
			{
				context.Request.Body.Position = 0;

				var reader = new StreamReader(context.Request.Body, Encoding.UTF8);

				var postBody = await reader.ReadToEndAsync();
				context.Request.Body.Position = 0;
				context.Items.Add("RequestBodySerialized", postBody);
			}

			await _next(context);
		}
	}
}
