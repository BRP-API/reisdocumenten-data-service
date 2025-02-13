using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using System.IO;
using System.Text;

namespace Rvig.HaalCentraalApi.Shared.Validation;

public static class ApiCallValidator
{
	public static async Task ValidateUnusableQueryParams(object model, HttpContext context)
	{
		var usedParams = new List<string>();
		if (context.Request.Method == HttpMethods.Get)
		{
			usedParams = context.Request.Query.Keys.ToList();
		}
		else
		{
			if (!context.Request.Body.CanSeek)
			{
				// We only do this if the stream isn't *already* seekable,
				// as EnableBuffering will create a new stream instance
				// each time it's called
				context.Request.EnableBuffering();
			}

			context.Request.Body.Position = 0;

			var reader = new StreamReader(context.Request.Body, Encoding.UTF8);

			var postBody = await reader.ReadToEndAsync();
			context.Request.Body.Position = 0;
			object? postObject = null;
			if (!string.IsNullOrWhiteSpace(postBody))
			{
				try
				{
					postObject = JsonConvert.DeserializeObject(postBody);
				}
				catch
				{
					throw new InvalidRequestBodyException();
				}
				if (postObject != null)
				{
					usedParams.AddRange(GetRequestParameterNames(postObject).ToList());
				}
				if (!context.Items.ContainsKey("RequestBodySerialized"))
				{
					context.Items.Add("RequestBodySerialized", postBody);
				}
			}
		}

		var validParams = model.GetType().GetProperties().Select(p => p.Name).ToList();

		var invalidParams = usedParams.Where(x => !validParams.Contains(x)).Select(x => new InvalidParams
		{
			Name = x,
			Code = nameof(InvalidParamCode.unknownParam),
			Reason = ValidationErrorMessages.UnexpectedParam
		});

		if (invalidParams.Any())
		{
			throw new InvalidParamsException(invalidParams);
		}
	}

	private static IEnumerable<string> GetRequestParameterNames(object postBodyObject)
	{
		var requestParams = new List<string>();

		GetProp((JObject)postBodyObject, requestParams);

		return requestParams;
	}

	private static void GetProp(JObject postBodyObject, IList<string> requestParams, string? parentPath = null)
	{
		foreach (var prop in postBodyObject.Properties())
		{
			var propName = !string.IsNullOrEmpty(parentPath) ? $"{parentPath}.{prop.Name}" : prop.Name;
			if (prop.Value is JObject propjObject)
			{
				GetProp(propjObject, requestParams, propName);
			}
			else
			{
				requestParams.Add(propName);
			}
		}
	}
}
