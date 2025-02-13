using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class NotFoundException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.NotFound; }

        public ErrorCode ErrorCode { get => ErrorCode.notFound; }

        public string Title { get; set; } = "Opgevraagde resource bestaat niet.";
        public string? Details { get; set; }

        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message)
        {
            Details = message;
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}