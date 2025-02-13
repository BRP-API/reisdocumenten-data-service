using Rvig.Data.Base.Postgres.DatabaseModels;

namespace Rvig.Data.Base.Postgres.Repositories.Queries;

public class RvIGBaseWhereMappingsHelper
{
	protected RvIGBaseWhereMappingsHelper()
	{
	}

	public static IDictionary<string, string> GetPersoonBaseMappings() => new Dictionary<string, string>()
	{
		// lo3_pl_persoon / DbPersoonActueelWrapper.Persoon
		["pers.pl_id"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.pl_id)}",
		["pers.persoon_type"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.persoon_type)}",
		["pers.stapel_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.stapel_nr)}",
		["pers.volg_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.volg_nr)}",
		["pers.a_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.a_nr)}",
		["pers.burger_service_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.burger_service_nr)}",
		["pers.voor_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.voor_naam)}",
		["pers.diak_voor_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.diak_voor_naam)}",
		["pers.titel_predicaat"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.titel_predicaat)}",
		["pers.geslachts_naam_voorvoegsel"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_naam_voorvoegsel)}",
		["pers.geslachts_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_naam)}",
		["pers.diak_geslachts_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.diak_geslachts_naam)}",
		["pers.geboorte_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_datum)}",
		["pers.geboorte_plaats"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_plaats)}",
		["pers.geboorte_land_code"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_land_code)}",
		["pers.geslachts_aand"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_aand)}",
		["pers.naam_gebruik_aand"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.naam_gebruik_aand)}",
		["pers.onderzoek_gegevens_aand"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_gegevens_aand)}",
		["pers.onderzoek_start_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_start_datum)}",
		["pers.onderzoek_eind_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_eind_datum)}",
		["pers.onjuist_ind"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onjuist_ind)}",
		["pers.relatie_start_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_start_datum)}",
		["pers.relatie_start_plaats"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_start_plaats)}",
		["pers.relatie_start_land_code"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_start_land_code)}",
		["pers.relatie_eind_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_datum)}",
		["pers.relatie_eind_plaats"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_plaats)}",
		["pers.relatie_eind_land_code"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_land_code)}",
		["pers.relatie_eind_reden"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_reden)}",
		["pers.verbintenis_soort"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.verbintenis_soort)}",
		["pers.familie_betrek_start_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.familie_betrek_start_datum)}",
		["pers.rni_deelnemer"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.rni_deelnemer)}",
		["pers.verdrag_oms"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.verdrag_oms)}",
		["pers.registratie_betrekking"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.registratie_betrekking)}",

		// Will no longer be get by main query but via dictionary.
		//["rni_deelnemer_pers.deelnemer_oms as pers_rni_deelnemer_omschrijving"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.pers_rni_deelnemer_omschrijving)}"
	};
	public static IDictionary<string, string> GetPersoonBeperktBaseMappings() => new Dictionary<string, string>()
	{
		// lo3_pl_persoon / DbPersoonActueelWrapper.Persoon
		["pers.pl_id"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.pl_id)}",
		["pers.persoon_type"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.persoon_type)}",
		["pers.stapel_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.stapel_nr)}",
		["pers.volg_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.volg_nr)}",
		["pers.burger_service_nr"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.burger_service_nr)}",
		["pers.voor_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.voor_naam)}",
		["pers.diak_voor_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.diak_voor_naam)}",
		["pers.titel_predicaat"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.titel_predicaat)}",
		["pers.geslachts_naam_voorvoegsel"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_naam_voorvoegsel)}",
		["pers.geslachts_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_naam)}",
		["pers.diak_geslachts_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.diak_geslachts_naam)}",
		["pers.geboorte_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_datum)}",
		["pers.geslachts_aand"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geslachts_aand)}",
		["pers.onderzoek_gegevens_aand"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_gegevens_aand)}",
		["pers.onderzoek_start_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_start_datum)}",
		["pers.onderzoek_eind_datum"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onderzoek_eind_datum)}",
		["pers.onjuist_ind"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.onjuist_ind)}",
		["pers.rni_deelnemer"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.rni_deelnemer)}",
		["pers.verdrag_oms"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.verdrag_oms)}",

		// Will no longer be get by main query but via dictionary.
		//["rni_deelnemer_pers.deelnemer_oms as pers_rni_deelnemer_omschrijving"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.pers_rni_deelnemer_omschrijving)}"
	};
	public static IDictionary<string, string> GetAdditionalPlRelationMappings() => GetPersoonBaseMappings().Concat(new Dictionary<string, string>
	{
		// Will no longer be get by main query but via dictionary.
		//["tp.titel_predicaat_oms"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.titel_predicaat_oms)}",
		//["tp.titel_predicaat_soort"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.titel_predicaat_soort)}",
		//["gebrte_plts.gemeente_naam as geboorte_plaats_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_plaats_naam)}",
		//["gebrte_land.land_naam as geboorte_land_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.geboorte_land_naam)}",
		//["rltie_strt_plts.gemeente_naam as relatie_start_plaats_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_start_plaats_naam)}",
		//["rltie_strt_lnd.land_naam as relatie_start_land_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_start_land_naam)}",
		//["rltie_eind_plts.gemeente_naam as relatie_eind_plaats_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_plaats_naam)}",
		//["rltie_eind_lnd.land_naam as relatie_eind_land_naam"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_land_naam)}",

		["rltie_eind_rdn.relatie_eind_reden_oms"] = $"{nameof(DbPersoonActueelWrapper.Persoon)}.{nameof(lo3_pl_persoon.relatie_eind_reden_oms)}"
	}).ToDictionary(x => x.Key, x => x.Value);
	public static IDictionary<string, string> GetProtocolleringMappings() => new Dictionary<string, string>()
	{
		["request_id"] = nameof(DbProtocollering.request_id),
		["request_datum"] = nameof(DbProtocollering.request_datum),
		["afnemer_code"] = nameof(DbProtocollering.afnemer_code),
		["pl_id"] = nameof(DbProtocollering.pl_id),
		["request_zoek_rubrieken"] = nameof(DbProtocollering.request_zoek_rubrieken),
		["request_gevraagde_rubrieken"] = nameof(DbProtocollering.request_gevraagde_rubrieken),
		["verwerkt"] = nameof(DbProtocollering.verwerkt),
	};
}