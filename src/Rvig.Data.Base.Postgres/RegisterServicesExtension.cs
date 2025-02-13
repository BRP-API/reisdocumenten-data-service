using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.Data.Base.Postgres.Authorisation;
using Rvig.Data.Base.Postgres.Helpers;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.Data.Base.Postgres.Services;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres;

public static class RegisterServicesExtension
{
    public static void ConfigureRvigDataBasePostgresServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.DatabaseSection));

        services.AddSingleton<IAutorisationRepo, AutorisationRepo>();
        services.AddSingleton<IProtocolleringRepo, ProtocolleringRepo>();
        services.AddSingleton<IProtocolleringService, ProtocolleringService>();
        services.AddSingleton<IDomeinTabellenRepo, DbDomeinTabellenRepo>();
        services.AddSingleton<IRvigDbHealthCheckRepo, RvigDbHealthCheckRepo>();

        services.AddSingleton<IDomeinTabellenHelper, DomeinTabellenHelper>();
        services.AddSingleton<IHealthCheckDatabaseConnectionService, HealthCheckDatabaseConnectionService>();
    }
}
