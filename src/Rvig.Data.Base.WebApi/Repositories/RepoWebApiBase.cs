using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.Extensions.Options;
using Rvig.Data.Base.WebApi.Options;
using Microsoft.AspNetCore.Http;

namespace Rvig.Data.Base.Gezag.Repositories
{
    public abstract class RepoWebApiBase
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly IOptions<WebApiOptions> _webApiOptions;

		protected RepoWebApiBase(IHttpContextAccessor httpContextAccessor, IOptions<WebApiOptions> webApiOptions)
		{
			_httpContextAccessor = httpContextAccessor;
			_webApiOptions = webApiOptions;
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

			HttpResponseMessage? response = await httpClient.SendAsync(httpRequest);

			if (response.StatusCode == HttpStatusCode.OK)
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
