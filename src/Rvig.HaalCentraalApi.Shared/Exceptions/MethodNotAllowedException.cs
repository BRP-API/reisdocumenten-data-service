using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
	public class MethodNotAllowedException : Exception, IHaalCentraalException
	{
		public HttpStatusCode HttpStatusCode { get => HttpStatusCode.MethodNotAllowed; }

		public ErrorCode ErrorCode { get => ErrorCode.methodNotAllowed; }

		public string Title { get; set; } = "Gebruikte bevragingsmethode is niet toegestaan.";
		public string? Details { get; set; }

		public MethodNotAllowedException() { }

		public MethodNotAllowedException(string message)
			: base(message)
		{
			Details = message;
		}

		public MethodNotAllowedException(string message, Exception innerException)
			: base(message, innerException)
		{
			Details = message;
		}

		protected MethodNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}