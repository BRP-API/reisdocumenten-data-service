namespace Rvig.Base.App.ResponseModels.HealthCheck;

public class HealthCheckResult
{
	public HealthCheckResult(int databaseAvailable)
	{
		DatabaseAvailable = databaseAvailable;
	}

	public int DatabaseAvailable { get; set; }
}
