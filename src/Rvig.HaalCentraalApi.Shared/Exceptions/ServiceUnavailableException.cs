using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions;

[Serializable]
public class ServiceUnavailableException : Exception, IHaalCentraalException
{
	public HttpStatusCode HttpStatusCode { get => HttpStatusCode.ServiceUnavailable; }

	public ErrorCode ErrorCode { get => ErrorCode.sourceUnavailable; }

	public string Title { get; set; } = "Bronservice is niet beschikbaar.";
	public string? Details { get; set; }

	public ServiceUnavailableException() { }

	public ServiceUnavailableException(string message)
		: base(message)
	{
		Details = message;
	}

	public ServiceUnavailableException(string message, Exception innerException)
		: base(message, innerException)
	{
		Details = message;
	}

	protected ServiceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}