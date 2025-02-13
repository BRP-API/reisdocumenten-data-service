using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public class ValidateVersionHeaderAttribute : ResultFilterAttribute
{
    private readonly string? _version;
	private readonly string _versionHeader = "api-version";

    public ValidateVersionHeaderAttribute()
    {
	}

    public ValidateVersionHeaderAttribute(string? version)
    {
        _version = version;
	}

    public override void OnResultExecuting(ResultExecutingContext context)
    {
		var version = _version ?? AppSettingsManager.Configuration?.GetValue<string>("ApiVersion");

		if (string.IsNullOrWhiteSpace(version))
		{
			throw new CustomInvalidOperationException("API version must always be set.");
		}
		context.HttpContext.Response.Headers.Add(_versionHeader, version);
        context.HttpContext.Items.Add(_versionHeader, version);

        context.HttpContext.Request.Headers.TryGetValue(_versionHeader, out var versionValue);
        var currentMajorVersion = int.Parse(version[0].ToString());

        if (versionValue.Any(x => !int.TryParse(x, out int requestedMajorVersion) || currentMajorVersion != requestedMajorVersion))
            throw new InvalidParamsException(new List<InvalidParams> { new InvalidParams { Name = _versionHeader, Code = "integer", Reason = "Waarde is geen geldige integer of bevat een versie die niet (meer) wordt ondersteund." } });

        base.OnResultExecuting(context);
    }
}
