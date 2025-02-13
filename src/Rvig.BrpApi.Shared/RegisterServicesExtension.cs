using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.BrpApi.Shared.Helpers;
using Rvig.BrpApi.Shared.Options;

namespace Rvig.BrpApi.Shared;

public static class RegisterServicesExtension
{
    public static void ConfigureRvigApiSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProtocolleringAuthorizationOptions>(configuration.GetSection(ProtocolleringAuthorizationOptions.ProtocolleringAuthorization));
        services.AddSingleton<ILoggingHelper, LoggingHelper>();
    }
}
