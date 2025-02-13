using Rvig.HaalCentraalApi.Reisdocumenten.ApiModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Reisdocumenten.Interfaces;
using Rvig.HaalCentraalApi.Shared.Validation;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Services;
using Rvig.HaalCentraalApi.Reisdocumenten.Fields;
using Rvig.HaalCentraalApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Reisdocumenten.ResponseModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Reisdocumenten.Helper;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Microsoft.Extensions.Options;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.HaalCentraalApi.Reisdocumenten.Services;
public interface IGbaReisdocumentenApiService
{
	Task<(ReisdocumentenQueryResponse reisdocumentenResponse, List<long>? plIds)> GetReisdocumenten(ReisdocumentenQuery model);
}

public class GbaReisdocumentenApiService : BaseApiService, IGbaReisdocumentenApiService
{
	protected IGetAndMapGbaReisdocumentenService _getAndMapReisdocumentenService;
	protected override ReisdocumentenFieldsSettings _fieldsSettings => new();
	private readonly ReisdocumentenApiToRubriekCategoryHelper _reisdocumentenApiToRubriekCategoryHelper = new();

	public GbaReisdocumentenApiService(IGetAndMapGbaReisdocumentenService getAndMapReisdocumentenService, IDomeinTabellenRepo domeinTabellenRepo, IProtocolleringService protocolleringService, ILoggingHelper loggingHelper, IOptions<ProtocolleringAuthorizationOptions> protocolleringAuthorizationOptions)
		: base(domeinTabellenRepo, protocolleringService, loggingHelper, protocolleringAuthorizationOptions)
	{
		_getAndMapReisdocumentenService = getAndMapReisdocumentenService;
	}

	public async Task<(ReisdocumentenQueryResponse reisdocumentenResponse, List<long>? plIds)> GetReisdocumenten(ReisdocumentenQuery model)
	{
		return model switch
		{
			RaadpleegMetReisdocumentnummer raadpleegMetReisdocumentnummer => await GetReisdocumenten(raadpleegMetReisdocumentnummer),
			ZoekMetBurgerservicenummer zoekMetBurgerservicenummer => await GetReisdocumenten(zoekMetBurgerservicenummer),
			_ => throw new CustomInvalidOperationException($"Onbekend type query: {model}"),
		};
	}

	private async Task<(RaadpleegMetReisdocumentnummerResponse reisdocumentenResponse, List<long>? plIds)> GetReisdocumenten(RaadpleegMetReisdocumentnummer model)
	{
		// Validate
		var reisdocumentnummers = model.reisdocumentnummer?.Distinct().ToList();

		if (reisdocumentnummers?.Any() == false)
		{
			throw new InvalidParamCombinationException("Combinatie van gevulde velden was niet correct. De correcte veld combinatie is reisdocumentnummer.");
		}

		_fieldsExpandFilterService.ValidateScope(typeof(GbaReisdocument), _fieldsSettings.GbaFieldsSettings, model.fields);

		// Get reisdocumenten
		var fieldsToUseForAuthorisations = model.fields.ConvertAll(field => _fieldsSettings.GbaFieldsSettings.ShortHandMappings.ContainsKey(field)
			? _fieldsSettings.GbaFieldsSettings.ShortHandMappings[field]
			: field);

		// Get reisdocumenten
		(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode) = await _getAndMapReisdocumentenService.GetReisdocumentByReisdocumentnummers(reisdocumentnummers!, _protocolleringAuthorizationOptions.Value.UseAuthorizationChecks);
		reisdocumentenPlIds = reisdocumentenPlIds?.Where(x => !GbaReisdocumentenApiHelper.IsReisdocumentNonExistent(x.reisdocument));
		// Filter response by fields
		if (model?.fields?.Any() == true)
		{
			reisdocumentenPlIds = reisdocumentenPlIds?.Select(x =>
			{
				x.reisdocument = _fieldsExpandFilterService.ApplyScope(x.reisdocument, string.Join(",", model.fields), _fieldsSettings.GbaFieldsSettings);
				GbaReisdocumentenApiHelper.FixInOnderzoek(model.fields, x.reisdocument);
				return x;
			}).ToList();

			if (_protocolleringAuthorizationOptions.Value.UseProtocollering)
			{
				await LogProtocolleringInDb(afnemerCode, reisdocumentenPlIds?.Select(x => x.pl_id).ToList(),
								_reisdocumentenApiToRubriekCategoryHelper.ConvertModelParamsToRubrieken(model)
									.Where(x => !string.IsNullOrWhiteSpace(x))
									.OrderBy(rubriek => rubriek.Substring(0))
									.ToList(),
								_reisdocumentenApiToRubriekCategoryHelper.ConvertFieldsToRubriekCategory(fieldsToUseForAuthorisations)
									.ConvertAll(x => x.rubriek)
									.Where(x => !string.IsNullOrWhiteSpace(x))
									.OrderBy(rubriek => rubriek.Substring(0))
									.ToList());
			}
		}

		var reisdocumenten = reisdocumentenPlIds?.ToList()?.ConvertAll(reisdocumentPlId => reisdocumentPlId.reisdocument) ?? new List<GbaReisdocument>();

		return (
			new RaadpleegMetReisdocumentnummerResponse { Reisdocumenten = reisdocumenten },
			reisdocumentenPlIds?.Select(x => x.pl_id).ToList()
		);
	}

	private async Task<(ZoekMetBurgerservicenummerResponse reisdocumentenResponse, List<long>? plIds)> GetReisdocumenten(ZoekMetBurgerservicenummer model)
	{
		// Validate
		if (string.IsNullOrEmpty(model.burgerservicenummer))
		{
			return (new ZoekMetBurgerservicenummerResponse(), new List<long>());
		}
		ValidationHelperBase.ValidateBurgerservicenummers(new List<string> { model.burgerservicenummer });

		_fieldsExpandFilterService.ValidateScope(typeof(GbaReisdocument), _fieldsSettings.GbaFieldsSettings, model.fields);

		// Get reisdocumenten
		var fieldsToUseForAuthorisations = model.fields.ConvertAll(field => _fieldsSettings.GbaFieldsSettings.ShortHandMappings.ContainsKey(field)
			? _fieldsSettings.GbaFieldsSettings.ShortHandMappings[field]
			: field);

		// Get reisdocumenten
		(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode) = await _getAndMapReisdocumentenService.GetReisdocumentByBurgerservicenummer(model.burgerservicenummer, _protocolleringAuthorizationOptions.Value.UseAuthorizationChecks);
		reisdocumentenPlIds = reisdocumentenPlIds?.Where(x => !GbaReisdocumentenApiHelper.IsReisdocumentNonExistent(x.reisdocument));

		// Filter response by fields
		if (model?.fields?.Any() == true)
		{
			reisdocumentenPlIds = reisdocumentenPlIds?.Select(x =>
			{
				x.reisdocument = _fieldsExpandFilterService.ApplyScope(x.reisdocument, string.Join(",", model.fields), _fieldsSettings.GbaFieldsSettings);
				GbaReisdocumentenApiHelper.FixInOnderzoek(model.fields, x.reisdocument);
				return x;
			}).ToList();

			if (_protocolleringAuthorizationOptions.Value.UseProtocollering)
			{
				await LogProtocolleringInDb(afnemerCode, reisdocumentenPlIds?.Select(x => x.pl_id).ToList(),
								_reisdocumentenApiToRubriekCategoryHelper.ConvertModelParamsToRubrieken(model)
									.Where(x => !string.IsNullOrWhiteSpace(x))
									.OrderBy(rubriek => rubriek.Substring(0))
									.ToList(),
								_reisdocumentenApiToRubriekCategoryHelper.ConvertFieldsToRubriekCategory(fieldsToUseForAuthorisations)
									.ConvertAll(x => x.rubriek)
									.Where(x => !string.IsNullOrWhiteSpace(x))
									.OrderBy(rubriek => rubriek.Substring(0))
									.ToList());
			}
		}

		var reisdocumenten = reisdocumentenPlIds?.ToList()?.ConvertAll(reisdocumentPlId => reisdocumentPlId.reisdocument) ?? new List<GbaReisdocument>();

		return (
			new ZoekMetBurgerservicenummerResponse { Reisdocumenten = reisdocumenten },
			reisdocumentenPlIds?.Select(x => x.pl_id).ToList()
		);
	}
}