using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_gemeente
{
    public int? gemeente_code { get; set; }
    public string? gemeente_naam { get; set; }
    public int? nieuwe_gemeente_code { get; set; }
}