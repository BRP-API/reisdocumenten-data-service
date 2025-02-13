using Microsoft.Extensions.Configuration;

namespace Rvig.BrpApi.Shared.Options;
public static class AppSettingsManager
{
    public static IConfiguration? Configuration { get; set; }
}