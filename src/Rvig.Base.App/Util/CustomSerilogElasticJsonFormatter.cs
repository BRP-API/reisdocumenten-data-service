﻿using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Newtonsoft.Json;
using Rvig.Base.App.Models;
using Rvig.BrpApi.Shared.ApiModels.Universal;

namespace Rvig.Base.App.Util
{
    public class CustomSerilogElasticJsonFormatter : ITextFormatter
	{
		private readonly string _apiVersionSettingValue;

		public CustomSerilogElasticJsonFormatter(string apiVersionSettingValue)
		{
			_apiVersionSettingValue = apiVersionSettingValue;
		}

		public void Format(LogEvent logEvent, TextWriter output)
		{
			var serializer = new JsonSerializer { Formatting = Formatting.None };
			var messageTemplate = !string.IsNullOrWhiteSpace(logEvent.MessageTemplate.Text) || logEvent.MessageTemplate.Text!.Equals("{}")
				? logEvent.RenderMessage()
				: null;

			//var requestId = _httpContext?.HttpContext?.TraceIdentifier;
			var logModel = new CustomElasticLogModel
			{
				Timestamp = logEvent.Timestamp,
				Log = new LogLoggingModelSegment
				{
					Level = logEvent.Level.ToString(),
					Logger = "API" // This is always API as this must contain the value of the application that creates the log.
				},
				Version = _apiVersionSettingValue, // Get API version from configuration manager.
			};

			if (logEvent.Properties.TryGetValue("uncaught", out var uncaught))
			{
				logModel.Uncaught = ((ScalarValue)uncaught).Value;
			}
			else if (logEvent.Exception != null)
			{
				logModel.Uncaught = logEvent.Exception;
			}

			if (logEvent.Properties.TryGetValue("Message", out var message) && ((ScalarValue)message).Value is string messageString && !string.IsNullOrWhiteSpace(messageString))
			{
				logModel.Message = messageString;
			}

			if (logEvent.Properties.TryGetValue("CustomRequestId", out var requestId))
			{
				logModel.Trace = new TraceLoggingModelSegment
				{
					// Serilog provides RequestId but it is unknown if this is set by Serilog or random generated by HTTP.
					Id = ((ScalarValue)requestId).Value as string // Get HTTP header here by name of Request-ID.
				};
			}
			if (logEvent.Properties.TryGetValue("RequestPath", out var requestPath))
			{
				logModel.Url = new UrlLoggingModelSegment
				{
					Path = ((ScalarValue)requestPath).Value as string // Get from HTTP Path.
				};
			}

			if (logEvent.Properties.TryGetValue("Response", out var response)
				 && response is ScalarValue responseScalar && responseScalar.Value is string responseString)
			{
				logModel.Http ??= new HttpLoggingModelSegment();
				logModel.Http.Response ??= new HttpLoggingModelSegment.HttpResponseLoggingModelSegment();
				if (responseString.ToLower().Contains(nameof(BadRequestFoutbericht.InvalidParams).ToLower()))
				{
					var responseDeserialized = JsonConvert.DeserializeObject<BadRequestFoutbericht>(responseString);
					logModel.Http.Response.Body = responseDeserialized != null ? responseDeserialized : null;
					logModel.Http.Response.StatusCode = responseDeserialized?.Status;
				}
				else
				{
					var responseDeserialized = JsonConvert.DeserializeObject<Foutbericht>(responseString);
					logModel.Http.Response.Body = responseDeserialized != null ? responseDeserialized : null;
					logModel.Http.Response.StatusCode = responseDeserialized?.Status;
				}
			}
			if (logEvent.Properties.TryGetValue("RequestMethod", out var requestMethod))
			{
				logModel.Http ??= new HttpLoggingModelSegment();
				logModel.Http.Request = new HttpLoggingModelSegment.HttpRequestLoggingModelSegment
				{
					Method = ((ScalarValue)requestMethod).Value as string // Get from HTTP method.
				};
			}
			if (logEvent.Properties.TryGetValue("StatusCode", out var statusCode))
			{
				logModel.Http ??= new HttpLoggingModelSegment();
				logModel.Http.Response ??= new HttpLoggingModelSegment.HttpResponseLoggingModelSegment();
				logModel.Http.Response.StatusCode = ((ScalarValue)statusCode).Value as int?; // Get from response.
			}
			if (logEvent.Properties.TryGetValue("Request", out var request))
			{
				logModel.Request ??= new RequestLoggingModelSegment();
				logModel.Request.Body ??= new RequestLoggingModelSegment.BodyLoggingModelSegment();
				logModel.Request.Body.Stringified = ((ScalarValue)request).Value as string;
			}
			if (logEvent.Properties.TryGetValue("Token", out var tokenClaims)
				&& tokenClaims is SequenceValue tokenClaimsSequence && tokenClaimsSequence?.Elements?.Any() == true)
			{
				var params2 = tokenClaimsSequence.Elements.Select(x => ((ScalarValue)x).Value as string)?.ToList();
				logModel.Token = params2?.Any() == true ? params2 : null; // Get token from OpenIdConnect handler.
			}

			serializer.Serialize(output, logModel);
			// This is to create a newline in the logger. If not used then all logs will be on the same line.
			output.WriteLine("");
		}
	}
}
