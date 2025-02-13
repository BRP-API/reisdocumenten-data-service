using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;

namespace Rvig.HaalCentraalApi.Shared.Helpers;

public interface ILoggingHelper
{
	void LogFatal(object? jsonObject, string? message = null);
	void LogError(object? jsonObject, string? message = null);
	void LogWarning(object? jsonObject, string? message = null);
	void LogInformation(object? jsonObject, string? message = null);
	void LogDebug(object? jsonObject, string? message = null);
	void LogVerbose(object? jsonObject, string? message = null);
}

public class LoggingHelper : ILoggingHelper
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public LoggingHelper(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}
	/// <summary>
	/// Formats and writes a verbose log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	public void LogVerbose(object? jsonObject, string? message = null) => LogJson(Log.Verbose, jsonObject, message);

	/// <summary>
	/// Formats and writes a debug log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	public void LogDebug(object? jsonObject, string? message = null) => LogJson(Log.Debug, jsonObject, message);

	/// <summary>
	/// Formats and writes an information log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	/// <example>logger.LogInformation(obj, address)</example>
	public void LogInformation(object? jsonObject, string? message = null) => LogJson(Log.Information, jsonObject, message);

	/// <summary>
	/// Formats and writes a warning log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	public void LogWarning(object? jsonObject, string? message = null) => LogJson(Log.Warning, jsonObject, message);

	/// <summary>
	/// Formats and writes an error log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	public void LogError(object? jsonObject, string? message = null) => LogJson(Log.Error, jsonObject, message);

	/// <summary>
	/// Formats and writes an fatal log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	public void LogFatal(object? jsonObject, string? message = null) => LogJson(Log.Fatal, jsonObject, message);

	/// <summary>
	/// Formats and writes a log message.
	/// </summary>
	/// <param name="jsonObject">Json object to log and escape.</c></param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	private void LogJson(Action<string> logMethod, object? jsonObject, string? message = null)
	{
		LogContext.Reset();
		if (jsonObject != null)
		{
			LogContext.PushProperty("loggingSource", nameof(LoggingHelper));
			switch (jsonObject)
			{
				case BadRequestFoutbericht:
				case Foutbericht:
					LogContext.PushProperty("Response", JsonConvert.SerializeObject(jsonObject));
					break;
				case string:
					LogContext.PushProperty("Message", jsonObject);
					break;
				default:
					if (!string.IsNullOrWhiteSpace(message))
					{
						LogContext.PushProperty("Message", message);
					}
					LogContext.PushProperty("uncaught", jsonObject);
					break;
			}

			if (_httpContextAccessor.HttpContext != null)
			{
				LogContext.PushProperty("RequestMethod", _httpContextAccessor.HttpContext.Request.Method);
				LogContext.PushProperty("StatusCode", _httpContextAccessor.HttpContext.Response.StatusCode);

				// Used for when we do not want request bodies to be serialized as strings.
				//if (_httpContextAccessor.HttpContext.Request.Body != null && _httpContextAccessor.HttpContext.Request.Body.CanRead)
				//{
					//if (!_httpContextAccessor.HttpContext.Request.Body.CanSeek)
					//{
					//	// We only do this if the stream isn't *already* seekable,
					//	// as EnableBuffering will create a new stream instance
					//	// each time it's called
					//	_httpContextAccessor.HttpContext.Request.EnableBuffering();
					//}

					//_httpContextAccessor.HttpContext.Request.Body.Position = 0;

					//var reader = new StreamReader(_httpContextAccessor.HttpContext.Request.Body, Encoding.UTF8);

					//var postBody = await reader.ReadToEndAsync();
					//_httpContextAccessor.HttpContext.Request.Body.Position = 0;
					//LogContext.PushProperty("Request", postBody);
				//}

				if (_httpContextAccessor.HttpContext.Items.Count > 0 && _httpContextAccessor.HttpContext.Items.ContainsKey("RequestBodySerialized"))
				{
					LogContext.PushProperty("Request", _httpContextAccessor.HttpContext.Items["RequestBodySerialized"]);
				}

				if (_httpContextAccessor.HttpContext.Request.Headers.Count > 0)
				{
					if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization")
					&& _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Contains("Bearer"))
					{
						var authorizationHeaderValue = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
						var userClientToken = authorizationHeaderValue?["Bearer ".Length..].Trim();
						var handler = new JwtSecurityTokenHandler();
						var jsonToken = handler.ReadToken(userClientToken);

						var userClaims = new List<string>();
						if (jsonToken is JwtSecurityToken token)
						{
							userClaims = token.Claims.Where(claim => claim.Type == "claims").Select(x => x.Value).ToList();
						}
						//var claims = Encoding.UTF8.GetString(Convert.FromBase64String(userClientToken!.Split(".")[1]));
						//LogContext.PushProperty("Token", "Hallo Frank!");
						LogContext.PushProperty("Token", userClaims);
					}
					if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Request-ID")
					&& !string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.Request.Headers["Request-ID"].ToString()))
					{
						var authorizationHeaderValue = _httpContextAccessor.HttpContext.Request.Headers["Request-ID"].ToString();
						LogContext.PushProperty("CustomRequestId", authorizationHeaderValue);
					}
				}
			}
			logMethod("");
		}
	}
}
internal class FoutberichtEnricher : ILogEventEnricher
{
	private readonly Foutbericht _foutbericht;

	public FoutberichtEnricher(Foutbericht foutbericht)
	{
		_foutbericht = foutbericht;
	}

	public virtual void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtType", _foutbericht.Type));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtTitle", _foutbericht.Title));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtCode", _foutbericht.Code));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtDetail", _foutbericht.Detail));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtStatus", _foutbericht.Status));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtInstance", _foutbericht.Instance));
	}
}

internal class BadRequestFoutberichtEnricher : FoutberichtEnricher
{
	private readonly BadRequestFoutbericht _badRequestFoutbericht;

	public BadRequestFoutberichtEnricher(BadRequestFoutbericht badRequestFoutbericht) : base(badRequestFoutbericht)
	{
		_badRequestFoutbericht = badRequestFoutbericht;
	}

	public override void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		base.Enrich(logEvent, propertyFactory);
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FoutberichtInvalidParams", JsonConvert.SerializeObject(_badRequestFoutbericht.InvalidParams)));
	}
}
