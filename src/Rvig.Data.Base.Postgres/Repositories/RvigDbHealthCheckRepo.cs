using Microsoft.Extensions.Options;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;

public interface IRvigDbHealthCheckRepo
{
	Task<string?> SendSimpleQuery();
}
public class RvigDbHealthCheckRepo : PostgresSqlQueryRepoBase<DbHealthCheckResult>, IRvigDbHealthCheckRepo
{
	public RvigDbHealthCheckRepo(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper) : base(databaseOptions, loggingHelper)
	{
		SetWhereMappings();
		SetMappings();
	}

	protected override void SetMappings() => CreateMappingsFromWhereMappings();
	protected override void SetWhereMappings() => WhereMappings = new Dictionary<string, string>
	{
		["'test' as selectresult"] = nameof(DbHealthCheckResult.SelectResult)
	};

	public async Task<string?> SendSimpleQuery()
	{
		var query = string.Format("select {0}", WhereMappings.Select(o => o.Key).Aggregate((i, j) => i + "," + j));
		var command = CreateDbCommand(query);

		try
		{
			var result = await GetFilterResultAsync(command);

			if (result?.Any() == true)
			{
				return result.Single().SelectResult;
			}

			return default;
		}
		catch
		{
			return default;
		}
	}
}