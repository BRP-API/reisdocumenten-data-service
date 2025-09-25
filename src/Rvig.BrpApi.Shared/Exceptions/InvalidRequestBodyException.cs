using System.Net;

namespace Rvig.BrpApi.Shared.Exceptions;

public class InvalidRequestBodyException : Exception, IHaalCentraalException
{
    public HttpStatusCode HttpStatusCode { get => HttpStatusCode.BadRequest; }

    public virtual ErrorCode ErrorCode { get => ErrorCode.paramsValidation; }

    public virtual string Title { get; set; } = "De bevraging bevat een fout.";
    public virtual string? Details { get; set; }

    public InvalidRequestBodyException()
    {
        Details = "Request body is geen geldige JSON.";
    }

    public InvalidRequestBodyException(string message, Exception innerException)
        : base(message, innerException)
    {
        Details = message;
    }

    public InvalidRequestBodyException(string? message) : base(message)
    {
        Details = message;
    }
}