using Brp.Shared.Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rvig.Data.Base;
using Rvig.Data.Base.Postgres;
using Rvig.HaalCentraalApi.Shared.Options;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using FluentValidation.AspNetCore;
using Rvig.Base.App.Middleware;
using Rvig.Base.App.Services;
using Rvig.Base.App.Authentication;
using FluentValidation;
using Rvig.HaalCentraalApi.Shared;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.Data.Base.WebApi;
using Serilog;
using Brp.Shared.Infrastructure.ProblemDetails;

namespace Rvig.Base.App;

public static class RvigBaseApp
{
	private static string? _currentAuthenticationType;
	/// <summary>
	/// Init app
	/// </summary>
	/// <param name="servicesToConfigure">Services to add as singletons. Key is interface, value is implementation.</param>
	/// <param name="validatorsToConfigure">Validator types of specific child app. Used for validation of post request objects.</param>
	/// <param name="controllerAssemblies">Controller resolving in other assemblies. E.g. typeof(EntitiesController).Assembly</param>
	public static void Init(IDictionary<Type, Type> servicesToConfigure, List<Type> validatorsToConfigure, Func<WebApplicationBuilder, bool> useAuthorizationLayerFunc, string apiName, IEnumerable<Assembly>? controllerAssemblies = null)
	{
		Log.Logger = SerilogHelpers.SetupSerilogBootstrapLogger();

		try
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			builder.SetupSerilog(Log.Logger);

			var appsettingsJsonFileName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()?.Equals("development") == true
				? $"appsettings.Development.json"
				: $"appsettings.json";

			builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile(appsettingsJsonFileName, optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

			builder.Services.ConfigureRvigRepoDataBaseServices(builder.Configuration);
			builder.Services.ConfigureRvigDataBasePostgresServices(builder.Configuration);
			builder.Services.ConfigureRvigDataBaseWebApiServices(builder.Configuration);
			builder.Services.ConfigureRvigApiSharedServices(builder.Configuration);
			builder.Services.AddHttpContextAccessor();

			ConfigureServices(builder.Services, builder.Configuration);

			// Loading services from child app. Personen, Reisdocumenten, Bewoningen or Historie.
			foreach (var servicePair in servicesToConfigure)
			{
				builder.Services.AddSingleton(servicePair.Key, servicePair.Value);
			}

			// Loading validators from child app.
			validatorsToConfigure.ForEach(validator => builder.Services.AddValidatorsFromAssemblyContaining(validator));

			bool useAuthorizationChecks = useAuthorizationLayerFunc(builder);

			var authnTypes = !string.IsNullOrWhiteSpace(builder.Configuration["AuthenticationTypes"])
				? builder.Configuration["AuthenticationTypes"]!.Split(",")
				: new List<string>().ToArray();
			string[] authenticationTypes = useAuthorizationChecks
				?  authnTypes
				: new List<string>().ToArray();
			builder = ConfigureAuth(builder, authenticationTypes);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddControllersWithViews();

			builder.Services.AddControllers(options =>
			{
				// Removes the POST main body mentioned during required errors.
				options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
			}).AddNewtonsoftJson();

			builder.Services.AddFluentValidationAutoValidation();

			builder.Services.Configure<MvcOptions>(options => options.Filters.Add(new ProducesAttribute(Application.Json)));
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.HttpContext.RequestServices.GetService<IErrorResponseService>()?.CreateBadRequestFoutbericht(context));
			});

			var app = builder.Build();

			app.SetupSerilogRequestLogging();
			app.UseMiddleware<UnhandledExceptionHandler>();

			app.UseExceptionHandler(new ExceptionHandlerOptions
			{
				AllowStatusCode404Response = true,
				ExceptionHandlingPath = "/error"
			});
			app.UseStatusCodePagesWithReExecute("/error/{0}");
			app.UseHsts();

			app.UseRouting();

			if (useAuthorizationChecks)
			{
				app.UseAuthentication();
				app.UseAuthorization();

				if (_currentAuthenticationType?.Equals("jwtbearer") == true)
				{
					app.UseMiddleware<JwtBearerWwwAuthenticateMiddleware>();
				}
			}

			// So we can grab the POST body to validate for unknown params. See ValidateUnusableQueryParamsAttribute.cs
			app.UseMiddleware<EnableRequestBodyBufferingMiddleware>();

			// It is a requirement of the GBA API to default to application/json when Content-Type isn't specified.
			app.UseMiddleware<ForceAcceptAndContentTypeHeadersWithValueMiddleware>();
			app.MapRazorPages();
			app.MapControllers();

			app.Run();
		}
		catch(Exception ex)
		{
			Log.Fatal(ex, $"{apiName} terminated unexpectedly.");
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static void ConfigureServices(IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.Configure<HaalcentraalApiOptions>(configurationManager.GetSection(HaalcentraalApiOptions.HaalcentraalApi));

		// Create a static manager to use in other classes where you cannot use IOptions in the constructor. Example being the ValidateVersionHeaderAttribute
		AppSettingsManager.Configuration = configurationManager;

		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddSingleton<IErrorResponseService, ErrorResponseService>();
		services.AddSingleton<IHealthCheckApiService, HealthCheckApiService>();
	}

	private static WebApplicationBuilder ConfigureAuth(WebApplicationBuilder builder, IEnumerable<string> authenticationTypes)
	{
		if (authenticationTypes == null || !authenticationTypes.Any())
		{
			// no authentication required
			return builder;
		}
		else if (authenticationTypes.Count() > 1)
		{
			throw new CustomInvalidOperationException("More than one authentication type was defined.");
		}
		_currentAuthenticationType = authenticationTypes.Single().ToLower();

		return _currentAuthenticationType switch
		{
			"basic" => ConfigureBasicAuth(builder),
			"openidconnect" => ConfigureOpenIdConnectAuth(builder),
			_ => throw new CustomInvalidOperationException("Unknown authentication type was defined.")
		};
	}

	private static WebApplicationBuilder ConfigureBasicAuth(WebApplicationBuilder builder)
	{
		builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", _ => { });
		builder.Services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());

		return builder;
	}

	private static WebApplicationBuilder ConfigureOpenIdConnectAuth(WebApplicationBuilder builder)
	{
		builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, OpenIdConnectAuthenticationHandler>("OpenIdConnectAuthentication", _ => { });
		builder.Services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder("OpenIdConnectAuthentication").RequireAuthenticatedUser().Build());

		return builder;
	}
}
