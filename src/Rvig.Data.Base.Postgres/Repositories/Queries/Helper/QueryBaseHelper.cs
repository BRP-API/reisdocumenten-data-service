using Npgsql;

namespace Rvig.Data.Base.Postgres.Repositories.Queries
{

	public class QueryBaseHelper
	{
		protected QueryBaseHelper()
		{
		}

		public static string AutorisatieQuery => @"select aut.afnemer_code, aut.geheimhouding_ind, aut.ad_hoc_medium, aut.ad_hoc_rubrieken, aut.adres_vraag_bevoegdheid, aut.bijzondere_betrekking_kind_verstrekken,
	voorwaarde.voorwaarde_regel
from lo3_autorisatie aut
	left join lo3_voorwaarde_regel_aut voorwaarde
	on aut.autorisatie_id = voorwaarde.autorisatie_id and voorwaarde.voorwaarde_type = '6'
where afnemer_code = @AFNEMERCODE
and (tabel_regel_start_datum is null or tabel_regel_start_datum <= @TODAY)
and (tabel_regel_eind_datum is null or tabel_regel_eind_datum > @TODAY)";
		public static string PersoonBaseQuery => @"select {0}
	from lo3_pl_persoon pers
	left join lo3_pl_verblijfplaats verblfpls
		on pers.pl_id = verblfpls.pl_id and verblfpls.volg_nr = 0
	left join lo3_adres adres
		on verblfpls.adres_id = adres.adres_id
	left join lo3_pl pl
		on pers.pl_id = pl.pl_id
	left join lo3_pl_overlijden overlijden
		on pers.pl_id = overlijden.pl_id and overlijden.volg_nr = 0
	left join lo3_pl_gezagsverhouding gezgvhdng
		on pers.pl_id = gezgvhdng.pl_id and gezgvhdng.volg_nr = 0
	left join lo3_pl_verblijfstitel verblftl
		on pers.pl_id = verblftl.pl_id and verblftl.volg_nr = 0
{1};";

		public static string PersoonBeperktBaseQuery => @"select {0}
from lo3_pl_persoon pers
	left join lo3_pl_verblijfplaats verblfpls
		on pers.pl_id = verblfpls.pl_id and verblfpls.volg_nr = 0
	left join lo3_adres adres
		on verblfpls.adres_id = adres.adres_id
	left join lo3_pl pl
		on pers.pl_id = pl.pl_id
{1};";
		public static string PersoonslijstByPlIds => @"select {0}
	from lo3_pl_persoon pers
		left join lo3_relatie_eind_reden rltie_eind_rdn
			on pers.relatie_eind_reden = rltie_eind_rdn.relatie_eind_reden
	where {1} and pers.onjuist_ind is null;";
		public static string PersoonNationaliteitenByPlIds => @"select natnltt.*, natnltt_oms.nationaliteit_oms
from lo3_pl_nationaliteit natnltt
	left join lo3_nationaliteit natnltt_oms
		on natnltt.nationaliteit_code = natnltt_oms.nationaliteit_code
where {0} and natnltt.volg_nr = 0 and onjuist_ind is null";

		public static (string where, NpgsqlParameter pgsqlParam) CreateBurgerservicenummerWhere(string bsn)
		{
			//  and pl.pl_blokkering_start_datum is null
			return ("where burger_service_nr = @BSN and persoon_type = 'P' and pers.stapel_nr = 0 and pers.volg_nr = 0 and((pl.bijhouding_opschort_reden is not null and pl.bijhouding_opschort_reden != 'W') or pl.bijhouding_opschort_reden is null)", new NpgsqlParameter("BSN", long.Parse(bsn)));
		}

		public static (string where, IEnumerable<NpgsqlParameter> parameters) CreateBurgerservicenummerWhere(IEnumerable<string> bsns)
		{
			//  and pl.pl_blokkering_start_datum is null
			(string where, IEnumerable<NpgsqlParameter> parameters) = CreateBsnPart(bsns);
			return ($"where {where} and persoon_type = 'P' and pers.stapel_nr = 0 and pers.volg_nr = 0 and((pl.bijhouding_opschort_reden is not null and pl.bijhouding_opschort_reden != 'W') or pl.bijhouding_opschort_reden is null)", parameters);
		}

		public static (string where, IEnumerable<NpgsqlParameter> parameters) CreateBurgerservicenummerGemeenteVanInschrijvingWhere(IEnumerable<string> bsns, string? gemeenteVanInschrijving)
		{
			(string where, List<NpgsqlParameter> parameters) whereClause = ("", new List<NpgsqlParameter>());
			(string where, IEnumerable<NpgsqlParameter> parameters) = CreateBurgerservicenummerWhere(bsns);
			whereClause.where = where;
			whereClause.parameters = parameters.ToList();
			if (string.IsNullOrWhiteSpace(gemeenteVanInschrijving))
			{
				return whereClause;
			}

			var gemCodeWhereParam = CreateGemeenteVanInschrijvingPart(int.Parse(gemeenteVanInschrijving));
			if (gemCodeWhereParam != default)
			{
				whereClause.where += $" and {gemCodeWhereParam.where}";
				whereClause.parameters.Add(gemCodeWhereParam.pgsqlParam);
			}

			return whereClause;
		}

		protected static (string where, NpgsqlParameter pgsqlParam) CreateGemeenteVanInschrijvingPart(int? gemeenteVanInschrijving)
		{
			if (!gemeenteVanInschrijving.HasValue)
			{
				return default;
			}
			var parameter = new NpgsqlParameter("GEMEENTEVANINSCHRIJVING", gemeenteVanInschrijving.Value);

			return ("verblfpls.inschrijving_gemeente_code = @GEMEENTEVANINSCHRIJVING", parameter);
		}

		private static (string where, IEnumerable<NpgsqlParameter> parameters) CreateBsnPart(IEnumerable<string> burgerservicenummers)
		{
			var parameters = new List<NpgsqlParameter>();

			if (burgerservicenummers.Count() == 1)
			{
				return ("burger_service_nr = @BSN",
					new List<NpgsqlParameter> { new NpgsqlParameter("BSN", long.Parse(burgerservicenummers.Single())) });
			}
			else
			{
				var bsnIndex = 0;
				var bsnParts = burgerservicenummers.Select(x =>
				{
					bsnIndex++;
					parameters.Add(new NpgsqlParameter($"BSN{bsnIndex}", long.Parse(x)));
					return $"@BSN{bsnIndex}";
				});
				var bsnPartsJoined = string.Join(", ", bsnParts);
				return ($"pers.burger_service_nr in ({bsnPartsJoined})", parameters);
			}
		}
	}
}
