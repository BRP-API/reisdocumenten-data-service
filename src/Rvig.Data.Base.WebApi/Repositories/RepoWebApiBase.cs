using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Options;
using Rvig.Data.Base.WebApi.Options;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Microsoft.AspNetCore.Http;

namespace Rvig.Data.Base.Gezag.Repositories
{
	public abstract class RepoWebApiBase
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly IOptions<WebApiOptions> _webApiOptions;
		private readonly ILoggingHelper _loggingHelper;

		protected RepoWebApiBase(IHttpContextAccessor httpContextAccessor, IOptions<WebApiOptions> webApiOptions, ILoggingHelper loggingHelper)
		{
			_httpContextAccessor = httpContextAccessor;
			_webApiOptions = webApiOptions;
			_loggingHelper = loggingHelper;
		}

		protected async Task<T?> GetResultFromHttpRequest<T>(string endpointUrl, string? parameters, HttpMethod httpMethod, List<(string Name, string Content)>? headers = null, object contentObject = null)
		{
			var url = endpointUrl + (string.IsNullOrWhiteSpace(parameters) ? "" : (parameters.StartsWith("/") ? parameters : $"/{parameters}"));
			var httpRequest = new HttpRequestMessage(httpMethod, url);

			if (httpMethod.Equals(HttpMethod.Post) && contentObject != null)
			{
				httpRequest.Content = new StringContent(JsonConvert.SerializeObject(contentObject));
				httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				httpRequest.Headers.Add("Accept", "*/*");
			}

			if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue("Request-ID", out var requestIdHeader) == true)
			{
				// Request-ID is a single string and not a list.
				httpRequest.Headers.Add("Request-ID", requestIdHeader.ToList().FirstOrDefault());
			}

			headers?.ForEach(x => httpRequest.Headers.Add(x.Name, x.Content));

			HttpClient httpClient = new();

			HttpResponseMessage? response;
			try
			{
				response = await httpClient.SendAsync(httpRequest);
			}
			catch (Exception ex)
			{
				_loggingHelper.LogError("Unexpected exception during webapi call. Error is: " + ex.Message + ".");
				throw;
			}

			if (response.StatusCode != HttpStatusCode.OK)
			{
				_loggingHelper.LogError("Webapi call unexpectedly ended in a fault. Error code was: " + response.StatusCode + ". Error body was: " + response.ReasonPhrase + ".");
			}
			else
			{
				var jsonResult = await response.Content.ReadAsStringAsync();

				if (string.IsNullOrEmpty(jsonResult))
				{
					return default;
				}

				var jSettings = new JsonSerializerSettings()
				{
					FloatParseHandling = FloatParseHandling.Decimal,
					NullValueHandling = NullValueHandling.Ignore,
					Converters = new List<JsonConverter>()
				};

				return JsonConvert.DeserializeObject<T>(jsonResult, jSettings);
			}

			return default;
		}
	}
}
