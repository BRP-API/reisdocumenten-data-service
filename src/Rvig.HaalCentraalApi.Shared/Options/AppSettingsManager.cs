using Microsoft.Extensions.Configuration;

namespace Rvig.HaalCentraalApi.Shared.Options;
public static class AppSettingsManager
{
	public static IConfiguration? Configuration { get; set; }
}