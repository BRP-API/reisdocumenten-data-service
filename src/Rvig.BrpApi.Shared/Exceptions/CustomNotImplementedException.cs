using System.Net;

namespace Rvig.BrpApi.Shared.Exceptions;

public class CustomNotImplementedException : NotImplementedException, IHaalCentraalException
{
    public HttpStatusCode HttpStatusCode { get => HttpStatusCode.InternalServerError; }

    public ErrorCode ErrorCode { get => ErrorCode.serverError; }

    public string Title { get; set; } = "Interne server fout.";
    public string? Details { get; set; }
    public CustomNotImplementedException() { }

    public CustomNotImplementedException(string? message) : base(message)
    {
        Details = message;
    }

    public CustomNotImplementedException(string? message, Exception? inner) : base(message, inner)
    {
        Details = message;
    }
}
