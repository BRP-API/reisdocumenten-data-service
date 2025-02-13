using Microsoft.AspNetCore.Mvc;
using Rvig.BrpApi.Shared.Validation;
using Rvig.BrpApi.Reisdocumenten.Services;
using Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.BrpApi.Reisdocumenten.ResponseModels.Reisdocumenten;
using Rvig.BrpApi.Shared.Controllers;

namespace Rvig.BrpApi.Reisdocumenten.Controllers;

[ApiController, Route("haalcentraal/api/reisdocumenten"), ValidateContentTypeHeader]
public class GbaApiReisdocumentenController : GbaApiBaseController
{
    private readonly IGbaReisdocumentenApiService _gbaService;

    public GbaApiReisdocumentenController(IGbaReisdocumentenApiService gbaService)
    {
        _gbaService = gbaService;
    }

    [HttpPost]
    [Route("reisdocumenten")]
    public async Task<ReisdocumentenQueryResponse> Reisdocumenten([FromBody] ReisdocumentenQuery model)
    {
        await ValidateUnusableQueryParams(model);
        (ReisdocumentenQueryResponse reisdocumentenResponse, List<long>? plIds) = await _gbaService.GetReisdocumenten(model);
        AddPlIdsToResponseHeaders(plIds);

        return reisdocumentenResponse;
    }
}