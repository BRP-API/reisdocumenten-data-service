using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.Data.Base.DatabaseModels;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.Data.Base.Postgres.Repositories.Queries;
using Rvig.Data.Reisdocumenten.Repositories.Queries;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Reisdocumenten.Repositories;
public interface IRvigReisdocumentenRepo
{
	Task<IEnumerable<lo3_pl_reis_doc>> GetReisdocumentByReisdocumentnummers(List<string> reisdocumentnummers);
	Task<IEnumerable<lo3_pl_reis_doc>> GetReisdocumentenByBurgerservicenummers(List<string> burgerservicenummers);
}

public class RvigReisdocumentenRepo : RvigRepoPostgresBase<lo3_pl_reis_doc>, IRvigReisdocumentenRepo
{
	public RvigReisdocumentenRepo(IOptions<DatabaseOptions> databaseOptions, IOptions<HaalcentraalApiOptions> haalcentraalApiOptions, ILoggingHelper loggingHelper) : base(databaseOptions, haalcentraalApiOptions, loggingHelper)
	{
		_haalcentraalApiOptions = haalcentraalApiOptions.Value;
	}

	protected override void SetMappings() => CreateMappingsFromWhereMappings();
	protected override void SetWhereMappings() => WhereMappings = RvIGReisdocumentenWhereMappingsHelper.GetReisdocumentMappings();

	public async Task<IEnumerable<lo3_pl_reis_doc>> GetReisdocumentBase((string where, IEnumerable<NpgsqlParameter> parameters) whereClauseAndParams/*, IDictionary<string, string>? additionalMappings = null*/)
	{
		var query = string.Format(ReisdocumentenQueryHelper.ReisdocumentBaseQuery, WhereMappings.Select(o => o.Key).Aggregate((i, j) => i + "," + j), whereClauseAndParams.where);
		var command = new NpgsqlCommand(query);
		command.Parameters.AddRange(whereClauseAndParams.parameters.ToArray());

		//if (additionalMappings != null)
		//{
		//	additionalMappings = CreateMappingsFromWhereMappings(additionalMappings);
		//}

		return (await GetFilterResultAsync(command/*, additionalMappings*/)).Where(reisdocument => reisdocument != null);
	}

	public Task<IEnumerable<lo3_pl_reis_doc>> GetReisdocumentByReisdocumentnummers(List<string> reisdocumentnummers)
		=> GetReisdocumentBase(ReisdocumentenQueryHelper.CreateReisdocumentnummersWhere(reisdocumentnummers));

	//pers.burger_service_nr
	public Task<IEnumerable<lo3_pl_reis_doc>> GetReisdocumentenByBurgerservicenummers(List<string> burgerservicenummers)
	{
		var whereClauseAndParams = QueryBaseHelper.CreateBurgerservicenummerWhere(burgerservicenummers);
		whereClauseAndParams.where += !string.IsNullOrWhiteSpace(whereClauseAndParams.where)
			? " and (reis.nl_reis_doc_weg_ind in ('R', '.') OR reis.nl_reis_doc_weg_ind is null)"
			: "where (reis.nl_reis_doc_weg_ind in ('R', '.') OR reis.nl_reis_doc_weg_ind is null)";
		return GetReisdocumentBase(whereClauseAndParams);
	}
}