using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class NotAcceptableException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.NotAcceptable; }

        public ErrorCode ErrorCode { get => ErrorCode.notAcceptable; }

        public string Title { get; set; } = "Gevraagde content type wordt niet ondersteund.";
        public string? Details { get; set; }

        public NotAcceptableException() { }

        public NotAcceptableException(string message)
            : base(message)
        {
            Details = message;
        }

        public NotAcceptableException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected NotAcceptableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}