using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_verblijfstitel : IVolgnummer
{
    public long pl_id { get; set; }
    [AlwaysAuthorized] public short volg_nr { get; set; }
    [RubriekElement("39.10")] public short? verblijfstitel_aand { get; set; }
    [RubriekElement("39.20")] public int? verblijfstitel_eind_datum { get; set; }
    [RubriekElement("39.30")] public int? verblijfstitel_start_datum { get; set; }
    [RubriekElement("83.10")] public int? onderzoek_gegevens_aand { get; set; }
    [RubriekElement("83.20")] public int? onderzoek_start_datum { get; set; }
    [RubriekElement("83.30")] public int? onderzoek_eind_datum { get; set; }
    [RubriekElement("84.10")] public string? onjuist_ind { get; set; }
    [RubriekElement("85.10")] public int? geldigheid_start_datum { get; set; }
    [RubriekElement("86.10")] public int? opneming_datum { get; set; }

    // joined omschrijvingen
    [RubriekElement("39.10")] public string? verblijfstitel_aand_oms { get; set; }
}