using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class UnauthorizationParameterException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.Forbidden; }

        public ErrorCode ErrorCode { get => ErrorCode.unauthorizedParameter; }

        public string Title { get; set; } = "U bent niet geautoriseerd voor de gebruikte parameter(s).";
        public string? Details { get; set; }

        public UnauthorizationParameterException() { }

        public UnauthorizationParameterException(string title, string message)
            : base(message)
        {
			Title = title;
            Details = message;
        }

        public UnauthorizationParameterException(string message)
            : base(message)
        {
            Details = message;
        }

        public UnauthorizationParameterException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected UnauthorizationParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}