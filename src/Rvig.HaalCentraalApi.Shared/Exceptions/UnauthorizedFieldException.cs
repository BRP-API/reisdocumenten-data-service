using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
	public class UnauthorizedFieldException : Exception, IHaalCentraalException
	{
		public HttpStatusCode HttpStatusCode { get => HttpStatusCode.Forbidden; }

		public ErrorCode ErrorCode { get => ErrorCode.unauthorizedField; }

		public string Title { get; set; } = "U bent niet geautoriseerd voor één of meerdere opgegeven field waarden.";
		public string? Details { get; set; }

		public UnauthorizedFieldException() { }

		public UnauthorizedFieldException(string title, string? message)
			: base(message)
		{
			Title = title;
			Details = message;
		}

		public UnauthorizedFieldException(string message)
			: base(message)
		{
			Details = message;
		}

		public UnauthorizedFieldException(string message, Exception innerException)
			: base(message, innerException)
		{
			Details = message;
		}

		protected UnauthorizedFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}