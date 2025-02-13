using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.HaalCentraalApi.Shared;

public static class RegisterServicesExtension
{
	public static void ConfigureRvigApiSharedServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<ProtocolleringAuthorizationOptions>(configuration.GetSection(ProtocolleringAuthorizationOptions.ProtocolleringAuthorization));
		services.AddSingleton<ILoggingHelper, LoggingHelper>();
	}
}
