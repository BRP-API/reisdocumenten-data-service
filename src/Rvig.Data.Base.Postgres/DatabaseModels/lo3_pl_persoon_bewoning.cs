using Rvig.Data.Base.Authorisation;
using System.Diagnostics.CodeAnalysis;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

[SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Record must conform the tablename in SQL source. Ignore C# casing convention because of this.")]
public record lo3_pl_persoon_bewoning : lo3_pl_persoon
{
	[RubriekCategory(08, 58), RubriekElement("10.30")] public int? begin_date { get; set; }
	[RubriekCategory(08, 58), RubriekElement("10.30")] public int? end_date { get; set; }
}