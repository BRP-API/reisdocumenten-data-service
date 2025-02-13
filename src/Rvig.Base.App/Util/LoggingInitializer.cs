using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Serilog.Core;

namespace Rvig.Base.App.Util;

public static class LoggingInitializer
{
	/// <summary>
	/// Setup logging framework in app.
	/// </summary>
	/// <param name="apiVersionSettingValue"></param>
	/// <param name="builder"></param>
	/// <param name="logNameAccessLog">Access log is used to log all functional errors/responses from the RvIG API's. This includes requests that my contain data of a person like a name or date of birth.</param>
	/// <param name="logNameApplicationLog">The same as access log in every way except for the data of a person in a request. Those are removed from this log.</param>
	/// <param name="logNameTraceLog">All remaining logging that aren't required or wanted in the functional logging or logging that is unexpected like a stack trace log.</param>
	public static void SetupLogging(string apiVersionSettingValue, WebApplicationBuilder builder, string logNameAccessLog = "access_log", string logNameApplicationLog = "applicatie_log", string logNameTraceLog = "trace_log")
	{
		var levelSwitch = new LoggingLevelSwitch();

		// Fatal, Error, Warning, Information, Debug, Verbose
		switch (builder.Configuration["Serilog:MinimumLevel:Default"])
		{
			case nameof(LogEventLevel.Fatal):
				levelSwitch.MinimumLevel = LogEventLevel.Fatal;
				break;
			case nameof(LogEventLevel.Warning):
				levelSwitch.MinimumLevel = LogEventLevel.Warning;
				break;
			case nameof(LogEventLevel.Information):
				levelSwitch.MinimumLevel = LogEventLevel.Information;
				break;
			case nameof(LogEventLevel.Debug):
				levelSwitch.MinimumLevel = LogEventLevel.Debug;
				break;
			case nameof(LogEventLevel.Verbose):
				levelSwitch.MinimumLevel = LogEventLevel.Verbose;
				break;
			// case nameof(LogEventLevel.Error):
			default:
				levelSwitch.MinimumLevel = LogEventLevel.Error;
				break;
		}

		var logFilePath = builder.Configuration["Serilog:LogFilePath"] ?? "Logs";

		// Add logging
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.ControlledBy(levelSwitch)
			.Enrich.FromLogContext()
			.WriteTo.Logger(l => l.Filter.ByIncludingOnly(logEvent => FilterLogging(logEvent)).WriteTo.File(new CustomSerilogElasticJsonFormatter(apiVersionSettingValue), $"{logFilePath}/{logNameAccessLog}"))
			.WriteTo.Logger(l => l.Filter.ByIncludingOnly(logEvent => false).WriteTo.File(new CustomSerilogElasticJsonFormatter(apiVersionSettingValue), $"{logFilePath}/{logNameApplicationLog}"))
			// TODO Create logging application_log where person information is removed entirely.
			//.WriteTo.Logger(l => l.Filter.ByIncludingOnly(logEvent => FilterLogging(logEvent)).WriteTo.File(new CustomSerilogElasticJsonFormatter(), $"{logFilePath}/applicatie_log"))
			// This is the logging bin sink. All logs that fall outside of the filtering above, will be written to a trace log.
			.WriteTo.Logger(l => l.Filter.ByIncludingOnly(logEvent => FilterEtcLogsLogging(logEvent)).WriteTo.File(new CustomSerilogElasticJsonFormatter(apiVersionSettingValue), $"{logFilePath}/{logNameTraceLog}"))
			.CreateLogger();

		builder.Logging.ClearProviders();
		builder.Logging.AddSerilog(Log.Logger);
	}

	private static bool FilterLogging(LogEvent logEvent)
	{
		var sourceContextNotNull = logEvent.Properties.TryGetValue("SourceContext", out var sourceContext);
		var loggingSourceExists = logEvent.Properties.TryGetValue("loggingSource", out var loggingSource);
		if (sourceContextNotNull && sourceContext?.ToString()?.StartsWith("Rvig.") == true)
		{
			return true;
		}
		else if (loggingSourceExists && loggingSource is ScalarValue scalarValue
		&& (!sourceContextNotNull || sourceContext?.ToString()?.StartsWith("Rvig.") == true))
		{
			return scalarValue.Value?.Equals(nameof(LoggingHelper)) == true;
		}

		return false;
	}

	/// <summary>
	/// All logs that should end up in a trace log because these are uncatched or not interesting are filtered here.
	/// </summary>
	/// <param name="logEvent"></param>
	/// <returns></returns>
	private static bool FilterEtcLogsLogging(LogEvent logEvent)
	{
		var eventIdCheck = logEvent.Properties.TryGetValue("EventId", out var eventId);
		var sourceContextNotNull = logEvent.Properties.TryGetValue("SourceContext", out var sourceContext);
		var loggingSourceExists = logEvent.Properties.TryGetValue("loggingSource", out var loggingSource);
		if (eventIdCheck
			&& !(sourceContextNotNull && sourceContext?.ToString()?.StartsWith("Rvig.") == true)
			&& !(loggingSourceExists && loggingSource is ScalarValue))
		{
			return true;
		}

		return false;
	}
}
