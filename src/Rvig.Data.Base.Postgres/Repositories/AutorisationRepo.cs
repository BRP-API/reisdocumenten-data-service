using Dapper;
using Microsoft.Extensions.Options;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Base.Providers;
using Rvig.Data.Base.Postgres.Repositories.Queries;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;
public interface IAutorisationRepo
{
    Task<DbAutorisatie?> GetByAfnemerCode(int afnemerCode);
}

public class AutorisationRepo : PostgresRepoBase, IAutorisationRepo
{
	public HaalcentraalApiOptions _haalcentraalApiOptions { get; set; }
    public ICurrentDateTimeProvider _currentDateTimeProvider { get; set; }

    public AutorisationRepo(IOptions<DatabaseOptions> databaseOptions, IOptions<HaalcentraalApiOptions> haalcentraalApiOptions, ICurrentDateTimeProvider currentDateTimeProvider
		, ILoggingHelper loggingHelper) : base(databaseOptions, loggingHelper)
    {
        _haalcentraalApiOptions = haalcentraalApiOptions.Value;
        _currentDateTimeProvider = currentDateTimeProvider;
	}

	public async Task<DbAutorisatie?> GetByAfnemerCode(int afnemerCode)
    {
        var currentDate = _currentDateTimeProvider.Today();
        var todayString = $"{currentDate.Year}{currentDate.Month.ToString().PadLeft(2, '0')}{currentDate.Day.ToString().PadLeft(2, '0')}";
        var todayInt = int.Parse(todayString);

        var query = QueryBaseHelper.AutorisatieQuery;

        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("AFNEMERCODE", afnemerCode);
        dynamicParameters.Add("TODAY", todayInt);

		var autorisaties = await DapperQueryAsync<DbAutorisatie>(query, dynamicParameters);

        if (autorisaties.Count() > 1)
        {
            throw new AuthorizationException($"Meer dan 1 autorisaties gevonden.");
        }

        return autorisaties.SingleOrDefault();
    }
}