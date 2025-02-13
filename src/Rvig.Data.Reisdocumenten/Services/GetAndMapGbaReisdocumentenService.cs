using Microsoft.AspNetCore.Http;
using Rvig.Data.Base.Postgres.Authorisation;
using Rvig.Data.Base.Postgres.Services;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Reisdocumenten.Mappers;
using Rvig.Data.Reisdocumenten.Repositories;
using Rvig.HaalCentraalApi.Reisdocumenten.ApiModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Reisdocumenten.Interfaces;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.Data.Base.DatabaseModels;
using Rvig.Data.Base.Services;

namespace Rvig.Data.Reisdocumenten.Services;
public class GetAndMapGbaReisdocumentenService : GetAndMapGbaServiceBase, IGetAndMapGbaReisdocumentenService
{
    private readonly IRvigReisdocumentenRepo _dbReisdocumentenRepo;
    private readonly IRvIGDataReisdocumentenMapper _reisdocumentenMapper;

	public GetAndMapGbaReisdocumentenService(IAutorisationRepo autorisationRepo, IRvigReisdocumentenRepo dbPersoonRepo, IRvIGDataReisdocumentenMapper reisdocumentenMapper,
		IHttpContextAccessor httpContextAccessor, IProtocolleringService protocolleringService)
		: base(httpContextAccessor, autorisationRepo, protocolleringService)
	{
		_dbReisdocumentenRepo = dbPersoonRepo;
		_reisdocumentenMapper = reisdocumentenMapper;
	}

	private async Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentenBase(List<string> identificaties, Func<List<string>, Task<IEnumerable<lo3_pl_reis_doc>>> getReisdocumentenDataObjectFunc, bool checkAuthorization)
	{
		Afnemer afnemer = new();
		DbAutorisatie autorisatie = new();
		if (checkAuthorization)
		{
			(afnemer, autorisatie) = await GetAfnemerAutorisatie();
		}

		var dbReisdocumenten = await getReisdocumentenDataObjectFunc(identificaties);
		var reisdocumentenPlIds = dbReisdocumenten.Where(dbReisdocument => FilterDatabaseReisdocumenten(dbReisdocument, checkAuthorization, afnemer.Gemeentecode))
			.Select(dbReisdocument =>
			{
				if (dbReisdocument == null)
				{
					return default;
				}

				// If we check authorization (checkAuthorization == true) then the following applies
				//		If binnengemeentelijk then autorisatie rubrieken in afnemer do not apply.
				//		By default this is true because if it reaches here it will be binnengemeentelijk. In the IsBinnenGemeentelijk method
				//		there will be a check for this and called before this line of code.
				// If we do not check authorization (checkAuthorization == false) then we will always accept all results without checking the GABA authorization.
				var persoonFiltered = AuthorisationService.Apply(dbReisdocument, autorisatie!, !checkAuthorization, new List<int>());

				if (persoonFiltered != null)
				{
					return (gbaReisdocument: _reisdocumentenMapper.MapReisdocument(dbReisdocument), dbReisdocument.pl_id);
				}

				return default;
			})
		.Where(x => x.gbaReisdocument != null);

		if (!reisdocumentenPlIds.Any())
		{
			return default;
		}

		// Check above already makes sure that a null value in the list isn't possible.
		return (reisdocumentenPlIds.Select(x => (x.gbaReisdocument, x.pl_id)), afnemer.Afnemerscode)!;
	}

	/// <summary>
	/// If reisdocument pl_bijhouding_opschort_reden has no value or if the value isn't equal to 'w' or 'f' AND this request is authorized then return reisdocument.
	/// </summary>
	/// <param name="dbReisdocument"></param>
	/// <param name="checkAuthorization"></param>
	/// <param name="gemeentecode"></param>
	/// <returns></returns>
	private bool FilterDatabaseReisdocumenten(lo3_pl_reis_doc dbReisdocument, bool checkAuthorization, short? gemeentecode)
	{
		return (string.IsNullOrWhiteSpace(dbReisdocument.pl_bijhouding_opschort_reden)
					|| (dbReisdocument.pl_bijhouding_opschort_reden?.ToLower().Equals("w") == false && dbReisdocument.pl_bijhouding_opschort_reden?.ToLower().Equals("f") == false))
			&& (!checkAuthorization || (checkAuthorization && IsBinnenGemeentelijk(gemeentecode, dbReisdocument.houder_inschrijving_gemeente_code?.ToString())));
	}

	public Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByReisdocumentnummers(List<string> reisdocumentnummers, bool checkAuthorization)
		=> GetReisdocumentenBase(reisdocumentnummers, _dbReisdocumentenRepo.GetReisdocumentByReisdocumentnummers, checkAuthorization);

	public Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByBurgerservicenummer(string burgerservicenummer, bool checkAuthorization)
		=> GetReisdocumentenBase(new List<string> { burgerservicenummer }, _dbReisdocumentenRepo.GetReisdocumentenByBurgerservicenummers, checkAuthorization);

	private bool IsBinnenGemeentelijk(short? afnemerGemeentecode, string? gemeenteVanInschrijving)
	{
		var isBinnenGemeentelijk = AuthorisationService.IsBinnenGemeentelijk(afnemerGemeentecode, gemeenteVanInschrijving);

		if (!isBinnenGemeentelijk)
		{
			throw new UnauthorizedException("U bent niet geautoriseerd voor deze vraag.", "Je mag alleen reisdocumenten van inwoners uit de eigen gemeente raadplegen.");
		}
		else
		{
			return isBinnenGemeentelijk;
		}
	}
}
