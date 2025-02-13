namespace Rvig.BrpApi.Shared.Interfaces;

public interface IHealthCheckDatabaseConnectionService
{
    Task<int> CheckDatabaseConnection();
}
