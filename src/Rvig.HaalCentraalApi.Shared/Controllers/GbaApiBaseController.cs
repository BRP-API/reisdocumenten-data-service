using Microsoft.AspNetCore.Mvc;
using Rvig.HaalCentraalApi.Shared.Validation;

namespace Rvig.HaalCentraalApi.Shared.Controllers;

public class GbaApiBaseController : ControllerBase
{
	protected Task ValidateUnusableQueryParams(object model)
	{
		return ApiCallValidator.ValidateUnusableQueryParams(model, HttpContext);
	}

	protected void AddPlIdsToResponseHeaders(List<long>? plIds)
	{
		if (plIds?.Any() == true)
		{
			Response.Headers.Add("x-geleverde-pls", string.Join(",", plIds.OrderBy(plId => plId)));
		}
	}

	protected void AddGemeenteCodesToResponseHeaders(List<string>? gemeenteCodes)
	{
		if (gemeenteCodes?.Any() == true)
		{
			Response.Headers.Add("x-geleverde-gemeentecodes", string.Join(",", gemeenteCodes.OrderBy(gemeenteCode => gemeenteCode)));
		}
	}
}