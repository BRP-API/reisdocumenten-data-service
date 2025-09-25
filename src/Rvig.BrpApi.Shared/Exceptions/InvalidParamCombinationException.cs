using Rvig.BrpApi.Shared.ApiModels.Universal;

namespace Rvig.BrpApi.Shared.Exceptions
{
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
    }
}