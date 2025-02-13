using System.Runtime.Serialization;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Serilog.Events;

namespace Rvig.Base.App.Models
{
	[DataContract]
	public class CustomElasticLogModel
	{
		[DataMember(Name = "@timestamp", EmitDefaultValue = false)]
		public DateTimeOffset? Timestamp { get; set; }

		[DataMember(Name = "trace", EmitDefaultValue = false)]
		public TraceLoggingModelSegment? Trace { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public UrlLoggingModelSegment? Url { get; set; }

		[DataMember(Name = "http", EmitDefaultValue = false)]
		public HttpLoggingModelSegment? Http { get; set; }

		[DataMember(Name = "log", EmitDefaultValue = false)]
		public LogLoggingModelSegment? Log { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string? Version { get; set; }

		[DataMember(Name = "token", EmitDefaultValue = false)]
		public List<string?>? Token { get; set; }

		[DataMember(Name = "request", EmitDefaultValue = false)]
		public RequestLoggingModelSegment? Request { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string? Message { get; set; }

		[DataMember(Name = "uncaught", EmitDefaultValue = false)]
		public object? Uncaught { get; set; }
	}

	[DataContract]
	public class TraceLoggingModelSegment
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string? Id { get; set; }
	}

	[DataContract]
	public class UrlLoggingModelSegment
	{
		[DataMember(Name = "path", EmitDefaultValue = false)]
		public string? Path { get; set; }
	}

	[DataContract]
	public class HttpLoggingModelSegment
	{
		[DataMember(Name = "request", EmitDefaultValue = false)]
		public HttpRequestLoggingModelSegment? Request { get; set; }

		[DataMember(Name = "response", EmitDefaultValue = false)]
		public HttpResponseLoggingModelSegment? Response { get; set; }

		[DataContract]
		public class HttpRequestLoggingModelSegment
		{
			[DataMember(Name = "method", EmitDefaultValue = false)]
			public string? Method { get; set; }
		}

		[DataContract]
		public class HttpResponseLoggingModelSegment
		{
			[DataMember(Name = "status_code", EmitDefaultValue = false)]
			public int? StatusCode { get; set; }

			[DataMember(Name = "body", EmitDefaultValue = false)]
			public Foutbericht? Body { get; set; }
		}
	}

	[DataContract]
	public class LogLoggingModelSegment
	{
		[DataMember(Name = "logger", EmitDefaultValue = false)]
		public string? Logger { get; set; }

		[DataMember(Name = "level", EmitDefaultValue = false)]
		public string? Level { get; set; }
	}

	[DataContract]
	public class RequestLoggingModelSegment
	{
		[DataMember(Name = "body", EmitDefaultValue = false)]
		public BodyLoggingModelSegment? Body { get; set; }

		[DataContract]
		public class BodyLoggingModelSegment
		{
			[DataMember(Name = "stringified", EmitDefaultValue = false)]
			public string? Stringified { get; set; }
		}
	}
}
