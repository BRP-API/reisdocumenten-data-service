using System.Net;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
    public interface IHaalCentraalException
    {
        HttpStatusCode HttpStatusCode { get; }
        ErrorCode ErrorCode { get; }
        string Title { get; set; }
        string? Details { get; set; }
    }
}
