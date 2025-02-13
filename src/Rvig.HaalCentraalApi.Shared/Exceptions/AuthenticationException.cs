using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions;

[Serializable]
public class AuthenticationException : Exception, IHaalCentraalException
{
	public HttpStatusCode HttpStatusCode { get => HttpStatusCode.Unauthorized; }

	public ErrorCode ErrorCode { get => ErrorCode.authentication; }

	public string Title { get; set; } = "Niet correct geauthenticeerd.";
	public string? Details { get; set; }

	public AuthenticationException() { }

	public AuthenticationException(string message)
		: base(message)
	{
		Details = message;
	}

	public AuthenticationException(string message, Exception innerException)
		: base(message, innerException)
	{
		Details = message;
	}

	protected AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}