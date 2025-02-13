using Npgsql;
using Rvig.Data.Base.Postgres.Repositories.Queries;

namespace Rvig.Data.Reisdocumenten.Repositories.Queries
{
	public class ReisdocumentenQueryHelper : QueryBaseHelper
	{
		public static string ReisdocumentBaseQuery => @"select {0}
	from lo3_pl_reis_doc reis
join lo3_pl_persoon pers on pers.pl_id = reis.pl_id and pers.persoon_type = 'P' and pers.stapel_nr = 0 and pers.volg_nr = 0
	left join lo3_pl pl
		on pers.pl_id = pl.pl_id
	left join lo3_pl_verblijfplaats verblfpls
		on pers.pl_id = verblfpls.pl_id and verblfpls.volg_nr = 0
{1};";

		public static (string where, NpgsqlParameter pgsqlParam) CreateReisdocumentnummerWhere(string reisdocumentnummer)
		{
			return ("where reis.nl_reis_doc_nr = @REISNR", new NpgsqlParameter("REISNR", reisdocumentnummer));
		}

		public static (string where, IEnumerable<NpgsqlParameter> parameters) CreateReisdocumentnummersWhere(IEnumerable<string> reisdocumentnummers)
		{
			//  and pl.pl_blokkering_start_datum is null
			(string where, IEnumerable<NpgsqlParameter> parameters) = CreateReisdocumentnummersPart(reisdocumentnummers);
			return ($"where {where}", parameters);
		}

		public static (string where, IEnumerable<NpgsqlParameter> parameters) CreateReisdocumentnummersGemeenteVanInschrijvingWhere(IEnumerable<string> reisdocumentnummers, string? gemeenteVanInschrijving)
		{
			(string where, List<NpgsqlParameter> parameters) whereClause = ("", new List<NpgsqlParameter>());
			(string where, IEnumerable<NpgsqlParameter> parameters) = CreateReisdocumentnummersWhere(reisdocumentnummers);
			whereClause.where = where;
			whereClause.parameters = parameters.ToList();
			if (string.IsNullOrWhiteSpace(gemeenteVanInschrijving))
			{
				return whereClause;
			}

			var gemCodeWhereParam = CreateGemeenteVanInschrijvingPart(int.Parse(gemeenteVanInschrijving));
			if (gemCodeWhereParam != default)
			{
				whereClause.where += $" and {gemCodeWhereParam.where}";
				whereClause.parameters.Add(gemCodeWhereParam.pgsqlParam);
			}

			return whereClause;
		}

		private static (string where, IEnumerable<NpgsqlParameter> parameters) CreateReisdocumentnummersPart(IEnumerable<string> reisdocumentnummers)
		{
			var parameters = new List<NpgsqlParameter>();

			if (reisdocumentnummers.Count() == 1)
			{
				return ("reis.nl_reis_doc_nr = @REISNR",
					new List<NpgsqlParameter> { new NpgsqlParameter("REISNR", reisdocumentnummers.Single()) });
			}
			else
			{
				var reisnrIndex = 0;
				var reisnrParts = reisdocumentnummers.Select(x =>
				{
					reisnrIndex++;
					parameters.Add(new NpgsqlParameter($"REISNR{reisnrIndex}", x));
					return $"@REISNR{reisnrIndex}";
				});
				var reisnrPartsJoined = string.Join(", ", reisnrParts);
				return ($"reis.nl_reis_doc_nr in ({reisnrPartsJoined})", parameters);
			}
		}
	}
}
