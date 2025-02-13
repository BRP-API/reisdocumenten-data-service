using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_verblijfstitel_aand
{
    public int? verblijfstitel_aand { get; set; }
    public string? verblijfstitel_aand_oms { get; set; }
}
