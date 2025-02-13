using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_nl_nat_verkrijg_verlies_reden
{
    public int? nl_nat_verkrijg_verlies_reden { get; set; }
    public string? nl_nat_reden_oms { get; set; }
}
