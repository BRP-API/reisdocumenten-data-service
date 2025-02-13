using Microsoft.AspNetCore.Mvc;
using Rvig.HaalCentraalApi.Shared.Validation;
using Rvig.HaalCentraalApi.Reisdocumenten.Services;
using Rvig.HaalCentraalApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Reisdocumenten.ResponseModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Shared.Controllers;

namespace Rvig.HaalCentraalApi.Reisdocumenten.Controllers;

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