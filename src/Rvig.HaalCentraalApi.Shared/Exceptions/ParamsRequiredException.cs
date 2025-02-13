using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class ParamsRequiredException : Exception, IHaalCentraalException
    {
        public HttpStatusCode HttpStatusCode { get => HttpStatusCode.BadRequest; }

        public ErrorCode ErrorCode { get => ErrorCode.paramsRequired; }

        public string Title { get; set; } = "Ten minste één parameter moet worden opgegeven.";
        public string? Details { get; set; }

        public ParamsRequiredException()
        {
            Details = "Bij deze bevraging zijn geen parameters gebruikt.";
        }

        public ParamsRequiredException(string message)
            : base(message)
        {
            Details = message;
        }

        public ParamsRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
            Details = message;
        }

		protected ParamsRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}