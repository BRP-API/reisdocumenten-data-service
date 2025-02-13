using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_verblijfplaats : IVolgnummer
{
    public long pl_id { get; set; }
    [AlwaysAuthorized] public short volg_nr { get; set; }
    [RubriekElement("09.10")] public short? inschrijving_gemeente_code { get; set; }
    public long? adres_id { get; set; }
    [RubriekElement("09.20")] public int? inschrijving_datum { get; set; }
    [RubriekElement("10.10")] public string? adres_functie { get; set; }
    [RubriekElement("10.20")] public string? gemeente_deel { get; set; }
    [RubriekElement("10.30")] public int? adreshouding_start_datum { get; set; }
    [RubriekElement("13.10")] public short? vertrek_land_code { get; set; }
    [RubriekElement("13.20")] public int? vertrek_datum { get; set; }
    [RubriekElement("13.30")] public string? vertrek_land_adres_1 { get; set; }
    [RubriekElement("13.40")] public string? vertrek_land_adres_2 { get; set; }
    [RubriekElement("13.50")] public string? vertrek_land_adres_3 { get; set; }
    [RubriekElement("14.10")] public short? vestiging_land_code { get; set; }
    [RubriekElement("14.20")] public int? vestiging_datum { get; set; }
    [RubriekElement("72.10")] public string? aangifte_adreshouding_oms { get; set; }
    [RubriekElement("75.10")] public short? doc_ind { get; set; }
    [RubriekElement("83.10")] public int? onderzoek_gegevens_aand { get; set; }
    [RubriekElement("83.20")] public int? onderzoek_start_datum { get; set; }
    [RubriekElement("83.30")] public int? onderzoek_eind_datum { get; set; }
    [RubriekElement("84.10")] public string? onjuist_ind { get; set; }
    [RubriekElement("85.10")] public int? geldigheid_start_datum { get; set; }
    [RubriekElement("86.10")] public int? opneming_datum { get; set; }
    [RubriekElement("88.10")] public short? rni_deelnemer { get; set; }
    [RubriekElement("88.20")] public string? verdrag_oms { get; set; }

    // joined omschrijvingen
    [RubriekElement("09.10")] public string? inschrijving_gemeente_naam { get; set; }
    [RubriekElement("14.10")] public string? vestiging_land_naam { get; set; }
    [RubriekElement("13.10")] public string? vertrek_land_naam { get; set; }
	[RubriekElement("88.10")] public string? verblfpls_rni_deelnemer_omschrijving { get; set; }
}