using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels
{
	[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
	public record lo3_adres
    {
        public long adres_id { get; init; }
        public short gemeente_code { get; init; }
        public string? gemeente_deel { get; init; }
        [RubriekElement("11.10")] public string? straat_naam { get; init; }
        [RubriekElement("11.10")] public string? diak_straat_naam { get; init; }
        [RubriekElement("11.20")] public int? huis_nr { get; init; }
        [RubriekElement("11.30")] public string? huis_letter { get; init; }
        [RubriekElement("11.40")] public string? huis_nr_toevoeging { get; init; }
        [RubriekElement("11.50")] public string? huis_nr_aand { get; init; }
        [RubriekElement("11.60")] public string? postcode { get; init; }
        [RubriekElement("12.10")] public string? locatie_beschrijving { get; init; }
        [RubriekElement("12.10")] public string? diak_locatie_beschrijving { get; init; }
        public DateTime creatie_dt { get; init; }
        [RubriekElement("11.15")] public string? open_ruimte_naam { get; init; }
        [RubriekElement("11.15")] public string? diak_open_ruimte_naam { get; init; }
        [RubriekElement("11.70")] public string? woon_plaats_naam { get; init; }
        [RubriekElement("11.70")] public string? diak_woon_plaats_naam { get; init; }
        [RubriekElement("11.80")] public string? verblijf_plaats_ident_code { get; init; }
        [RubriekElement("11.90")] public string? nummer_aand_ident_code { get; init; }
    }
}
