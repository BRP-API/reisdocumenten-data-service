using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_gezagsverhouding : IVolgnummer
{
    public long pl_id { get; set; }
    [AlwaysAuthorized] public short volg_nr { get; set; }
    [RubriekElement("32.10")] public string? minderjarig_gezag_ind { get; set; }
    [RubriekElement("33.10")] public short? curatele_register_ind { get; set; }
    [RubriekElement("82.10")] public short? doc_gemeente_code { get; set; }
    [RubriekElement("82.20")] public int? doc_datum { get; set; }
    [RubriekElement("82.30")] public string? doc_beschrijving { get; set; }
    [RubriekElement("83.10")] public int? onderzoek_gegevens_aand { get; set; }
    [RubriekElement("83.20")] public int? onderzoek_start_datum { get; set; }
    [RubriekElement("83.30")] public int? onderzoek_eind_datum { get; set; }
    [RubriekElement("84.10")] public string? onjuist_ind { get; set; }
    [RubriekElement("85.10")] public int? geldigheid_start_datum { get; set; }
    [RubriekElement("86.10")] public int? opneming_datum { get; set; }

	// joined omschrijvingen
	[RubriekElement("32.10")] public string? minderjarig_gezag_oms { get; set; }
}