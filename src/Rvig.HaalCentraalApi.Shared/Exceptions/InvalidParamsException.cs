using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using System.Net;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions;

[Serializable]
public class InvalidParamsException : Exception, IHaalCentraalException
{
    public HttpStatusCode HttpStatusCode { get => HttpStatusCode.BadRequest; }

    public virtual ErrorCode ErrorCode { get => ErrorCode.paramsValidation; }

    public virtual string Title { get; set; } = "Een of meerdere parameters zijn niet correct.";
    public virtual string? Details { get; set;  }

    public IEnumerable<InvalidParams> InvalidParams { get; }

    public InvalidParamsException()
    {
        InvalidParams = new List<InvalidParams>();
    }

    public InvalidParamsException(IEnumerable<InvalidParams> invalidParams)
    {
		Details = $"De foutieve parameter(s) zijn: {string.Join(", ", invalidParams.Select(invalidParam => invalidParam.Name).Distinct().OrderBy(param => param))}.";
        InvalidParams = invalidParams;
    }

    public InvalidParamsException(string message, IEnumerable<InvalidParams> invalidParams)
        : base(message)
    {
        Details = message;
        InvalidParams = invalidParams;
    }

    public InvalidParamsException(string message, Exception innerException)
        : base(message, innerException)
    {
        Details = message;
        InvalidParams = new List<InvalidParams>();
    }

    public InvalidParamsException(string message, Exception innerException, IEnumerable<InvalidParams> invalidParams)
        : base(message, innerException)
    {
        Details = message;
        InvalidParams = invalidParams;
    }

    public InvalidParamsException(string? message) : base(message)
    {
        Details = message;
        InvalidParams = new List<InvalidParams>();
    }

    protected InvalidParamsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        InvalidParams = new List<InvalidParams>();
    }
}
