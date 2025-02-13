using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions;

[Serializable]
public class CustomInvalidOperationException : InvalidOperationException, IHaalCentraalException
{
	public HttpStatusCode HttpStatusCode { get => HttpStatusCode.InternalServerError; }

	public ErrorCode ErrorCode { get => ErrorCode.serverError; }

	public string Title { get; set; } = "Interne server fout.";
	public string? Details { get; set; }
	public CustomInvalidOperationException() { }

	public CustomInvalidOperationException(string? message) : base(message)
	{
		Details = message;
	}

	public CustomInvalidOperationException(string? message, Exception? inner) : base(message, inner)
	{
		Details = message;
	}

	protected CustomInvalidOperationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
	{
	}
}