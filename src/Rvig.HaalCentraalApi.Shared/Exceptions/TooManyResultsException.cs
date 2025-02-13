using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class TooManyResultsException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.BadRequest; }

        public ErrorCode ErrorCode { get => ErrorCode.tooManyResults; }

        public string Title { get; set; } = "Teveel zoekresultaten.";
        public string? Details { get; set; }

        public int MaxResult { get; set; }

        public TooManyResultsException() { }

        public TooManyResultsException(string message)
            : base(message)
        {
            Details = message;
        }

        public TooManyResultsException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

        protected TooManyResultsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}