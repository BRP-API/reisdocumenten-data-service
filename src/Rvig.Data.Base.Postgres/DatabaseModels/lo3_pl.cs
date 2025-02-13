using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl
{
    public long pl_id { get; set; }
    [RubriekCategory(7), RubriekElement("66.20")] public int? pl_blokkering_start_datum { get; set; }
    [RubriekCategory(7), RubriekElement("67.10")] public int? bijhouding_opschort_datum { get; set; }
    [RubriekCategory(7), RubriekElement("67.20")] public string? bijhouding_opschort_reden { get; set; }
    [RubriekCategory(7), RubriekElement("68.10")] public int? gba_eerste_inschrijving_datum { get; set; }
    [RubriekCategory(7), RubriekElement("69.10")] public short? pk_gemeente_code { get; set; }
    [RubriekCategory(7), RubriekElement("70.10")] public short? geheim_ind { get; set; }
    [RubriekCategory(7), RubriekElement("87.10")] public string? volledig_geconverteerd_pk { get; set; }
    [RubriekCategory(13), RubriekElement("31.10")] public short? europees_kiesrecht_aand { get; set; }
    [RubriekCategory(13), RubriekElement("31.20")] public int? europees_kiesrecht_datum { get; set; }
    [RubriekCategory(13), RubriekElement("31.30")] public int? europees_uitsluit_eind_datum { get; set; }
    [RubriekCategory(13), RubriekElement("38.10")] public string? kiesrecht_uitgesl_aand { get; set; }
    [RubriekCategory(13), RubriekElement("38.20")] public int? kiesrecht_uitgesl_eind_datum { get; set; }
    [RubriekCategory(13), RubriekElement("82.10")] public short? kiesrecht_doc_gemeente_code { get; set; }
    [RubriekCategory(13), RubriekElement("82.20")] public int? kiesrecht_doc_datum { get; set; }
    [RubriekCategory(13), RubriekElement("82.30")] public string? kiesrecht_doc_beschrijving { get; set; }
    public long? mutatie_activiteit_id { get; set; }
    public DateTime creatie_dt { get; set; }
    //public DateTime timestamp { get; set; } // What is this Hans?
    [RubriekCategory(7), RubriekElement("80.10")] public short? versie_nr { get; set; }
    [RubriekCategory(7), RubriekElement("80.20")] public long? stempel_dt { get; set; }
    [RubriekCategory(7), RubriekElement("71.10")] public int? verificatie_datum { get; set; }
    [RubriekCategory(7), RubriekElement("71.20")] public string? verificatie_oms { get; set; }
    [RubriekCategory(7), RubriekElement("88.10")] public short? rni_deelnemer { get; set; }
    [RubriekCategory(7), RubriekElement("88.20")] public string? verdrag_oms { get; set; }
	[RubriekElement("88.10")] public string? pl_rni_deelnemer_omschrijving { get; set; }
}