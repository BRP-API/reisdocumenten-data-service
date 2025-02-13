using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
	[Serializable]
    public class InvalidParamCombinationException : InvalidParamsException
    {
        public override ErrorCode ErrorCode { get => ErrorCode.paramsCombination; }

        public override string Title { get; set; } = "Minimale combinatie van parameters moet worden opgegeven.";
        public override string? Details { get; set; }

        public InvalidParamCombinationException() : base()
        {
        }

        public InvalidParamCombinationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidParamCombinationException(string? message) : base(message)
        {
        }

        public InvalidParamCombinationException(IEnumerable<InvalidParams> invalidParams) : base(invalidParams)
        {
        }

        public InvalidParamCombinationException(string message, IEnumerable<InvalidParams> invalidParams) : base(message, invalidParams)
        {
        }

        public InvalidParamCombinationException(string message, Exception innerException, IEnumerable<InvalidParams> invalidParams) : base(message, innerException, invalidParams)
        {
        }

        protected InvalidParamCombinationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}