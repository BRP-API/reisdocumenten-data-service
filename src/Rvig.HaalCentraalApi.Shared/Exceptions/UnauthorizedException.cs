using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.Forbidden; }

        public ErrorCode ErrorCode { get => ErrorCode.unauthorized; }

        public string Title { get; set; } = "U bent niet geautoriseerd voor het gebruik van deze API.";
        public string? Details { get; set; }

        public UnauthorizedException() { }

        public UnauthorizedException(string title, string message)
            : base(message)
        {
			Title = title;
            Details = message;
        }

        public UnauthorizedException(string message)
            : base(message)
        {
            Details = message;
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}