using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_persoon : IVolgnummer
{
    public long pl_id { get; set; }
    [AlwaysAuthorized] public string? persoon_type { get; set; }
    [AlwaysAuthorized] public short stapel_nr { get; set; }
    [AlwaysAuthorized] public short volg_nr { get; set; }
    [RubriekElement("01.10")] public long? a_nr { get; set; }
    [RubriekElement("01.20")] public long? burger_service_nr { get; set; }
    [RubriekElement("02.10")] public string? voor_naam { get; set; }
    [RubriekElement("02.10")] public string? diak_voor_naam { get; set; }
    [RubriekElement("02.20")] public string? titel_predicaat { get; set; }
    [RubriekElement("02.30")] public string? geslachts_naam_voorvoegsel { get; set; }
    [RubriekElement("02.40")] public string? geslachts_naam { get; set; }
    [RubriekElement("02.40")] public string? diak_geslachts_naam { get; set; }
    [RubriekElement("03.10")] public int? geboorte_datum { get; set; }
    [RubriekElement("03.20")] public string? geboorte_plaats { get; set; }
    [RubriekElement("03.30")] public short? geboorte_land_code { get; set; }
    [RubriekElement("04.10")] public string? geslachts_aand { get; set; }
    [RubriekElement("61.10")] public string? naam_gebruik_aand { get; set; }
    [RubriekElement("81.10")] public short? akte_register_gemeente_code { get; set; }
    [RubriekElement("81.20")] public string? akte_nr { get; set; }
    [RubriekElement("82.10")] public short? doc_gemeente_code { get; set; }
    [RubriekElement("82.20")] public int? doc_datum { get; set; }
    [RubriekElement("82.30")] public string? doc_beschrijving { get; set; }
    [RubriekElement("83.10")] public int? onderzoek_gegevens_aand { get; set; }
    [RubriekElement("83.20")] public int? onderzoek_start_datum { get; set; }
    [RubriekElement("83.30")] public int? onderzoek_eind_datum { get; set; }
    [RubriekCategory(5, 55), RubriekElement("84.10")] public string? onjuist_ind { get; set; }
    [RubriekElement("85.10")] public int? geldigheid_start_datum { get; set; }
    [RubriekElement("86.10")] public int? opneming_datum { get; set; }
    [RubriekCategory(5, 55), RubriekElement("06.10")] public int? relatie_start_datum { get; set; }
    [RubriekCategory(5, 55), RubriekElement("06.20")] public string? relatie_start_plaats { get; set; }
    [RubriekCategory(5, 55), RubriekElement("06.30")] public short? relatie_start_land_code { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.10")] public int? relatie_eind_datum { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.20")] public string? relatie_eind_plaats { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.30")] public short? relatie_eind_land_code { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.40")] public string? relatie_eind_reden { get; set; }
    [RubriekElement("15.10")] public string? verbintenis_soort { get; set; }
    [RubriekElement("62.10")] public int? familie_betrek_start_datum { get; set; }
    [RubriekElement("20.10")] public long? vorig_a_nr { get; set; }
    [RubriekElement("20.20")] public long? volgend_a_nr { get; set; }
    [RubriekElement("88.10")] public short? rni_deelnemer { get; set; }
    [RubriekElement("88.20")] public string? verdrag_oms { get; set; }
    [RubriekElement("89.10")] public string? registratie_betrekking { get; set; }

    // joined omschrijvingen
    [RubriekElement("02.20")] public string? titel_predicaat_oms { get; set; }
    [RubriekElement("02.20")] public string? titel_predicaat_soort { get; set; }
    [RubriekElement("03.20")] public string? geboorte_plaats_naam { get; set; }
    [RubriekElement("03.30")] public string? geboorte_land_naam { get; set; }
    [RubriekCategory(5, 55), RubriekElement("06.20")] public string? relatie_start_plaats_naam { get; set; }
    [RubriekCategory(5, 55), RubriekElement("06.30")] public string? relatie_start_land_naam { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.20")] public string? relatie_eind_plaats_naam { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.30")] public string? relatie_eind_land_naam { get; set; }
    [RubriekCategory(5, 55), RubriekElement("07.40")] public string? relatie_eind_reden_oms { get; set; }
	[RubriekElement("88.10")] public string? pers_rni_deelnemer_omschrijving { get; set; }
}