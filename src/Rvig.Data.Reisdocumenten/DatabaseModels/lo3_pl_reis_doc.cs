using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_reis_doc
{
    public long pl_id { get; set; }
    [AlwaysAuthorized] public short stapel_nr { get; set; }
    [RubriekElement("35.10")] public string? nl_reis_doc_soort { get; set; }
    [RubriekElement("35.20")] public string? nl_reis_doc_nr { get; set; }
    [RubriekElement("35.30")] public int? nl_reis_doc_uitgifte_datum { get; set; }
    [RubriekElement("35.40")] public string? nl_reis_doc_autoriteit_code { get; set; }
    [RubriekElement("35.50")] public int? nl_reis_doc_geldig_eind_datum { get; set; }
    [RubriekElement("35.60")] public int? nl_reis_doc_weg_datum { get; set; }
    [RubriekElement("35.70")] public string? nl_reis_doc_weg_ind { get; set; }
    public short? nl_reis_doc_houder_lengte { get; set; }
    [RubriekElement("36.10")] public short? nl_reis_doc_signalering { get; set; }
    public short? buitenland_reis_doc_aand { get; set; }
    [RubriekElement("82.10")] public short? doc_gemeente_code { get; set; }
    [RubriekElement("82.20")] public int? doc_datum { get; set; }
    [RubriekElement("82.30")] public string? doc_beschrijving { get; set; }
    [RubriekElement("83.10")] public int? onderzoek_gegevens_aand { get; set; }
    [RubriekElement("83.20")] public int? onderzoek_start_datum { get; set; }
    [RubriekElement("83.30")] public int? onderzoek_eind_datum { get; set; }
    [RubriekElement("85.10")] public int? geldigheid_start_datum { get; set; }
    [RubriekElement("86.10")] public int? opneming_datum { get; set; }

	[RubriekCategory(01,51), RubriekElement("01.20")] public long? burger_service_nr { get; set; }
	[RubriekCategory(8), RubriekElement("09.10")] public short? houder_inschrijving_gemeente_code { get; set; }
	[RubriekCategory(8), RubriekElement("09.10")] public short? houder_inschrijving_gemeente_naam { get; set; }

	// joined omschrijvingen
	[RubriekCategory(7), RubriekElement("70.10")] public short? pl_geheim_ind { get; set; }
	[RubriekCategory(01,51), RubriekElement("83.10")] public int? pers_onderzoek_aand { get; set; }
	[RubriekCategory(01,51), RubriekElement("83.20")] public int? pers_onderzoek_start_datum { get; set; }
	[RubriekCategory(01, 51), RubriekElement("83.30")] public int? pers_onderzoek_eind_datum { get; set; }
	[RubriekCategory(7), RubriekElement("67.10")] public int? pl_bijhouding_opschort_datum { get; set; }
	[RubriekCategory(7), RubriekElement("67.20")] public string? pl_bijhouding_opschort_reden { get; set; }
}