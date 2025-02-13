using Rvig.Data.Base.DatabaseModels;
using Rvig.Data.Base.Postgres.Repositories.Queries;

namespace Rvig.Data.Reisdocumenten.Repositories.Queries;

public class RvIGReisdocumentenWhereMappingsHelper : RvIGBaseWhereMappingsHelper
{
	public static IDictionary<string, string> GetReisdocumentMappings() => new Dictionary<string, string>()
	{
		["reis.pl_id"] = nameof(lo3_pl_reis_doc.pl_id),
		["reis.stapel_nr"] = nameof(lo3_pl_reis_doc.stapel_nr),
		["reis.nl_reis_doc_soort"] = nameof(lo3_pl_reis_doc.nl_reis_doc_soort),
		["reis.nl_reis_doc_nr"] = nameof(lo3_pl_reis_doc.nl_reis_doc_nr),
		["reis.nl_reis_doc_uitgifte_datum"] = nameof(lo3_pl_reis_doc.nl_reis_doc_uitgifte_datum),
		["reis.nl_reis_doc_autoriteit_code"] = nameof(lo3_pl_reis_doc.nl_reis_doc_autoriteit_code),
		["reis.nl_reis_doc_geldig_eind_datum"] = nameof(lo3_pl_reis_doc.nl_reis_doc_geldig_eind_datum),
		["reis.nl_reis_doc_weg_datum"] = nameof(lo3_pl_reis_doc.nl_reis_doc_weg_datum),
		["reis.nl_reis_doc_weg_ind"] = nameof(lo3_pl_reis_doc.nl_reis_doc_weg_ind),
		["reis.nl_reis_doc_houder_lengte"] = nameof(lo3_pl_reis_doc.nl_reis_doc_houder_lengte),
		["reis.nl_reis_doc_signalering"] = nameof(lo3_pl_reis_doc.nl_reis_doc_signalering),
		["reis.buitenland_reis_doc_aand"] = nameof(lo3_pl_reis_doc.buitenland_reis_doc_aand),
		["reis.doc_gemeente_code"] = nameof(lo3_pl_reis_doc.doc_gemeente_code),
		["reis.doc_datum"] = nameof(lo3_pl_reis_doc.doc_datum),
		["reis.doc_beschrijving"] = nameof(lo3_pl_reis_doc.doc_beschrijving),
		["reis.onderzoek_gegevens_aand"] = nameof(lo3_pl_reis_doc.onderzoek_gegevens_aand),
		["reis.onderzoek_start_datum"] = nameof(lo3_pl_reis_doc.onderzoek_start_datum),
		["reis.onderzoek_eind_datum"] = nameof(lo3_pl_reis_doc.onderzoek_eind_datum),
		["reis.geldigheid_start_datum"] = nameof(lo3_pl_reis_doc.geldigheid_start_datum),
		["reis.opneming_datum"] = nameof(lo3_pl_reis_doc.opneming_datum),

		// Houder
		["pers.burger_service_nr"] = nameof(lo3_pl_reis_doc.burger_service_nr),
		["verblfpls.inschrijving_gemeente_code as houder_inschrijving_gemeente_code"] = nameof(lo3_pl_reis_doc.houder_inschrijving_gemeente_code),
		["pl.geheim_ind as pl_geheim_ind"] = nameof(lo3_pl_reis_doc.pl_geheim_ind),
		["pers.onderzoek_gegevens_aand as pers_onderzoek_aand"] = nameof(lo3_pl_reis_doc.pers_onderzoek_aand),
		["pers.onderzoek_start_datum as pers_onderzoek_start_datum"] = nameof(lo3_pl_reis_doc.pers_onderzoek_start_datum),
		["pers.onderzoek_eind_datum as pers_onderzoek_eind_datum"] = nameof(lo3_pl_reis_doc.pers_onderzoek_eind_datum),
		["pl.bijhouding_opschort_datum as pl_bijhouding_opschort_datum"] = nameof(lo3_pl_reis_doc.pl_bijhouding_opschort_datum),
		["pl.bijhouding_opschort_reden as pl_bijhouding_opschort_reden"] = nameof(lo3_pl_reis_doc.pl_bijhouding_opschort_reden),
	};
}