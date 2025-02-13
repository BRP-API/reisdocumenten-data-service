using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
    [Serializable]
    public class AuthorizationException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.Forbidden; }

        public ErrorCode ErrorCode { get => ErrorCode.authorization; }

        public string Title { get; set; } = "U bent niet geautoriseerd voor deze operatie.";
        public string? Details { get; set; }

        public AuthorizationException() { }

        public AuthorizationException(string title, string? message)
            : base(message)
        {
			Title = title;
            Details = message;
        }

        public AuthorizationException(string message)
            : base(message)
        {
            Details = message;
        }

        public AuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}