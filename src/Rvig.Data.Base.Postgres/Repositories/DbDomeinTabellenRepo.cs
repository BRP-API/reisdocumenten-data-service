using Dapper;
using Microsoft.Extensions.Options;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;
// lo3_gemeente is not correct
public class DbDomeinTabellenRepo : PostgresRepoBase, IDomeinTabellenRepo
{
	public DbDomeinTabellenRepo(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper) : base(databaseOptions, loggingHelper)
	{
	}

	public async Task<IEnumerable<(string? code, string? omschrijving, string? soort)>> GetAllAdellijkeTitelsPredikaten()
	{
		string query = "select titel_predicaat, titel_predicaat_oms, titel_predicaat_soort from lo3_titel_predicaat";

		var adellijkeTitelsPredicaten = await DapperQueryAsync<lo3_titel_predicaat>(query);
		return adellijkeTitelsPredicaten.Select(titelPredicaat => (titelPredicaat.titel_predicaat, titelPredicaat.titel_predicaat_oms, titelPredicaat.titel_predicaat_soort));
	}

	public async Task<IEnumerable<(int? code, string? omschrijving, int? nieuweCode)>> GetAllGemeenten()
	{
		string query = "select gemeente_code, gemeente_naam, nieuwe_gemeente_code from lo3_gemeente";

		var gemeentes = await DapperQueryAsync<lo3_gemeente>(query);
		return gemeentes.Select(gemeente => (gemeente.gemeente_code, gemeente.gemeente_naam, gemeente.nieuwe_gemeente_code));
	}

	public async Task<IEnumerable<(int? code, string? omschrijving)>> GetAllLanden()
	{
		string query = "select land_code, land_naam from lo3_land";

		var landen = await DapperQueryAsync<lo3_land>(query);
		return landen.Select(land => (land.land_code, land.land_naam));
	}

	public async Task<IEnumerable<(int? code, string? omschrijving)>> GetAllRedenOpnemenBeeindigenNationaliteit()
	{
		string query = "select nl_nat_verkrijg_verlies_reden, nl_nat_reden_oms from lo3_nl_nat_verkrijg_verlies_reden";

		var redenen = await DapperQueryAsync<lo3_nl_nat_verkrijg_verlies_reden>(query);
		return redenen.Select(reden => (reden.nl_nat_verkrijg_verlies_reden, reden.nl_nat_reden_oms));
	}

	public async Task<IEnumerable<(int? code, string? omschrijving)>> GetAllVerblijfstitels()
	{
		string query = "select verblijfstitel_aand, verblijfstitel_aand_oms from lo3_verblijfstitel_aand";

		var verblijfstitels = (await DapperQueryAsync<lo3_verblijfstitel_aand>(query));
		return verblijfstitels.Select(verblijfstitel => (verblijfstitel.verblijfstitel_aand, verblijfstitel.verblijfstitel_aand_oms));
	}

	public async Task<IEnumerable<(int? code, string? omschrijving)>> GetAllRniDeelnemers()
	{
		string query = "select deelnemer_code, deelnemer_oms from lo3_rni_deelnemer";

		var rniDeelnemers = await DapperQueryAsync<lo3_rni_deelnemer>(query);
		return rniDeelnemers.Select(rniDeelnemer => (rniDeelnemer.deelnemer_code, rniDeelnemer.deelnemer_oms));
	}

	public async Task<IEnumerable<(string? code, string? omschrijving)>> GetAllGezagsverhoudingen()
	{
		string query = "select gezagsverhouding_code, gezagsverhouding_oms from lo3_gezagsverhouding";

		var gezagsverhoudingen = await DapperQueryAsync<lo3_gezagsverhouding>(query);
		return gezagsverhoudingen.Select(gezag => (gezag.gezagsverhouding_code, gezag.gezagsverhouding_oms));
	}

	public Task<string?> GetGemeenteNaam(int? gemeenteCode)
	{
		if (!gemeenteCode.HasValue)
		{
			throw new CustomNotImplementedException("No gemeentecode received.");
		}
		long.TryParse(gemeenteCode.Value.ToString(), out var gemCode);

		return GetGemeenteNaam(gemCode);
	}

	public virtual async Task<string?> GetGemeenteNaam(long gemeenteCode)
	{
		string query = "select gemeente_naam from lo3_gemeente where gemeente_code = @GEMEENTECODE";

		var dynamicParameters = new DynamicParameters();
		dynamicParameters.Add("GEMEENTECODE", gemeenteCode);

		var gemeentes = await DapperQueryAsync<lo3_gemeente>(query, dynamicParameters);
		return gemeentes.SingleOrDefault()?.gemeente_naam;
	}

	public virtual async Task<bool> VoorvoegselExist(string voorvoegsel)
	{
		string query = "select 1 as voorvoegsel_exists from lo3_voorvoegsel where lower(voorvoegsel) = lower(@VOORVOEGSEL) limit 1";

		var dynamicParameters = new DynamicParameters();
		dynamicParameters.Add("VOORVOEGSEL", voorvoegsel);

		var exists = await DapperQueryAsync<bool>(query, dynamicParameters);
		return exists.Any();
	}
}