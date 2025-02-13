using Rvig.Data.Base.Postgres.Repositories;
using Rvig.HaalCentraalApi.Shared.Interfaces;

namespace Rvig.Data.Base.Postgres.Services;

public class HealthCheckDatabaseConnectionService : IHealthCheckDatabaseConnectionService
{
	private readonly IRvigDbHealthCheckRepo _rvigDbHealthCheckRepo;

	public HealthCheckDatabaseConnectionService(IRvigDbHealthCheckRepo rvigDbHealthCheckRepo)
	{
		_rvigDbHealthCheckRepo = rvigDbHealthCheckRepo;
	}

	public async Task<int> CheckDatabaseConnection()
	{
		var result = await _rvigDbHealthCheckRepo.SendSimpleQuery();

		return !string.IsNullOrWhiteSpace(result) ? 0 : 1;
	}
}
