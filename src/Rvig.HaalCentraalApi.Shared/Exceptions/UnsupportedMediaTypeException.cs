using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class UnsupportedMediaTypeException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.UnsupportedMediaType; }

        public ErrorCode ErrorCode { get => ErrorCode.unsupportedMediaType; }

        public string Title { get; set; } = "Media Type wordt niet ondersteund.";
        public string? Details { get; set; }

        public UnsupportedMediaTypeException() { }

        public UnsupportedMediaTypeException(string message)
            : base(message)
        {
            Details = message;
        }

        public UnsupportedMediaTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected UnsupportedMediaTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}