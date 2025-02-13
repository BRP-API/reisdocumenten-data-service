using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rvig.Base.App.ResponseModels.HealthCheck;
using Rvig.Base.App.Services;

namespace Rvig.Base.App.Controllers;

[AllowAnonymous, ApiController, Route("haalcentraal/api/health")]
public class HealthCheckController : ControllerBase
{
	private readonly IHealthCheckApiService _healthCheckApiService;

	public HealthCheckController(IHealthCheckApiService healthCheckApiService)
	{
		_healthCheckApiService = healthCheckApiService;
	}

	[HttpGet]
	[Route("check")]
	public Task<HealthCheckResult> Check()
	{
		return _healthCheckApiService.CheckConnections();
	}
}
