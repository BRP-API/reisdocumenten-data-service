using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.Data.Base.WebApi.Options;

namespace Rvig.Data.Base.WebApi;

public static class RegisterServicesExtension
{
	public static void ConfigureRvigDataBaseWebApiServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.WebApi));
	}
}
