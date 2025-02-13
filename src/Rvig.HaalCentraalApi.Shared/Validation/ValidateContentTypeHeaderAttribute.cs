using Microsoft.AspNetCore.Mvc.Filters;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using System.Net.Http.Headers;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public class ValidateContentTypeHeaderAttribute : ResultFilterAttribute
{
    private readonly string[] _acceptedAcceptHeaders;
    private readonly string[] _acceptedContentTypes;
    private readonly string _default;

    public ValidateContentTypeHeaderAttribute(bool usesHal = false)
    {
		_acceptedAcceptHeaders = new string[] { "*/*", "*/*;charset=utf-8", "*/*; charset=utf-8", "application/json", "application/json; charset=utf-8", "application/json;charset=utf-8" };
		_acceptedContentTypes = new string[] { "application/json", "application/json; charset=utf-8", "application/json;charset=utf-8" };
        _default = usesHal ? "application/hal+json; charset=utf-8" : "application/json; charset=utf-8";
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var request = context.HttpContext.Request;

        if (request.Headers.Accept.Count > 0 && !request.Headers.Accept.Any(x => _acceptedAcceptHeaders.Contains(x?.ToLower())))
        {
            throw new NotAcceptableException($"Ondersteunde content type: {_default}.");
		}
        else if (request.Headers.ContentType.Count > 0 && !request.Headers.ContentType.Any(x => _acceptedContentTypes.Contains(x?.ToLower())))
		{
            throw new UnsupportedMediaTypeException($"Ondersteunde content type: {_default}.");
		}

		context.HttpContext.Response.OnStarting((mediaType) =>
        {
            context.HttpContext.Response.ContentType = new MediaTypeHeaderValue((string)mediaType)
            {
                CharSet = "utf-8"
            }.ToString();

            return Task.FromResult(0);
        }, _default.Replace("; charset=utf-8", ""));

        base.OnResultExecuting(context);
    }
}
