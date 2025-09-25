using Microsoft.AspNetCore.Mvc;
using Rvig.BrpApi.Shared.Validation;

namespace Rvig.BrpApi.Shared.Controllers;

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
			Response.Headers["x-geleverde-pls"] = string.Join(",", plIds.OrderBy(plId => plId));
        }
    }

    protected void AddGemeenteCodesToResponseHeaders(List<string>? gemeenteCodes)
    {
        if (gemeenteCodes?.Any() == true)
        {
			Response.Headers["x-geleverde-gemeentecodes"] = string.Join(",", gemeenteCodes.OrderBy(gemeenteCode => gemeenteCode));
        }
    }
}