namespace Rvig.HaalCentraalApi.Shared.Interfaces;

public interface IHealthCheckDatabaseConnectionService
{
	Task<int> CheckDatabaseConnection();
}
