using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.Data.Base.Providers;

namespace Rvig.Data.Base;

public static class RegisterServicesExtension
{
    public static void ConfigureRvigRepoDataBaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentDateTimeProvider, DateTimeTodayProvider>();
    }
}
