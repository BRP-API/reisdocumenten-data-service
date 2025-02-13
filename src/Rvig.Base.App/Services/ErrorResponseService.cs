using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;
using Rvig.HaalCentraalApi.Shared.Validation;
using System.Net;
using System.Net.Http.Headers;

namespace Rvig.Base.App.Services;

public interface IErrorResponseService
{
	Foutbericht CreateFoutBericht(HttpContext httpContext, HttpResponse response, Exception exception, string originalPath);
	BadRequestFoutbericht CreateBadRequestFoutbericht(ActionContext actionContext);
	BadRequestFoutbericht CreateBadRequestFoutbericht(HttpContext httpContext, InvalidParamsException exception, string path);
}

/// <summary>
/// This service is used by the GlobalErrorController or the InvalidModelStateResponseFactory which is configured in StartUp
/// to create a haalcentraal error response from either an exception or an invalid modelstate.
/// </summary>
public class ErrorResponseService : IErrorResponseService
{
	private readonly IOptions<HaalcentraalApiOptions> _haalcentraalApiOptions;
	private readonly ILoggingHelper _loggingHelper;

	private IDictionary<Type, InvalidParamCode> _typeCodeEnumDictionary => new Dictionary<Type, InvalidParamCode>
	{
		{ typeof(string), InvalidParamCode._string },
		{ typeof(int), InvalidParamCode.integer },
		{ typeof(bool), InvalidParamCode.boolean },
		{ typeof(DateOnly), InvalidParamCode.date },
		{ typeof(DateTime), InvalidParamCode.date }
	};

	public ErrorResponseService(IOptions<HaalcentraalApiOptions> haalcentraalApiOptions, ILoggingHelper loggingHelper)
	{
		_haalcentraalApiOptions = haalcentraalApiOptions;
		_loggingHelper = loggingHelper;
	}

	public Foutbericht CreateFoutBericht(HttpContext httpContext, HttpResponse response, Exception exception, string originalPath)
	{
		var httpStatusCode = HttpStatusCode.InternalServerError;
		var errorCode = ErrorCode.serverError;
		var title = "Interne server fout.";
		string? details = null;

		if (exception is IHaalCentraalException haalCentraalException)
		{
			httpStatusCode = haalCentraalException.HttpStatusCode;
			errorCode = haalCentraalException.ErrorCode;
			title = haalCentraalException.Title;
			details = haalCentraalException.Details;
		}
		else
		{
			title = exception.Message;
			details = exception.InnerException?.Message ?? exception.StackTrace;
		}

		var intHttpStatusCode = (int)httpStatusCode;
		var instance = GetInstance(originalPath + httpContext.Request.QueryString);

		response.StatusCode = intHttpStatusCode;
		SetHeaders(httpContext);

		var foutBericht = new Foutbericht
		{
			Code = errorCode.ToString(),
			Detail = details,
			Title = title,
			Status = intHttpStatusCode,
			Type = GetType(intHttpStatusCode),
			Instance = instance
		};
		// _loggingHelper.LogError(foutBericht);
		return foutBericht;
	}

	public BadRequestFoutbericht CreateBadRequestFoutbericht(ActionContext actionContext)
	{
		var modelState = actionContext.ModelState;
		var httpContext = actionContext.HttpContext;
		var pathAndQuery = httpContext.Request.Path + httpContext.Request.QueryString;
		var instance = GetInstance(pathAndQuery);

		var validationState = modelState.Root.ValidationState;
		Exception exception;
		if (validationState.Equals(ModelValidationState.Invalid)
			|| validationState.Equals(ModelValidationState.Unvalidated)
					&& actionContext is ActionExecutingContext executingContext
					&& executingContext.ActionArguments.Count == 0
					&& modelState.All(bodyParam => !bodyParam.Value?.Errors
																.Any(error => !string.IsNullOrWhiteSpace(ValidationErrorMessages.GetInvalidParamCode(error.ErrorMessage))) == true))
		{
			exception = new InvalidRequestBodyException();
		}
		else
		{
			// create an invalidParam for each error in the modelstate
			var invalidParams = modelState.Keys.SelectMany(key => modelState[key]!.Errors.Select(error => CreateInvalidParam(actionContext, key, error.ErrorMessage)))
				.OrderBy(x => x.Reason);

			exception = new InvalidParamsException(invalidParams);
		}

		actionContext.HttpContext.Response.StatusCode = 400;
		SetHeaders(actionContext.HttpContext);

		var foutBericht = CreateBadRequestFoutbericht(exception, instance);
		// _loggingHelper.LogError(foutBericht);
		return foutBericht;
	}

	public BadRequestFoutbericht CreateBadRequestFoutbericht(HttpContext httpContext, InvalidParamsException exception, string path)
	{
		var instance = GetInstance(path + httpContext.Request.QueryString);

		httpContext.Response.StatusCode = 400;
		SetHeaders(httpContext);

		return CreateBadRequestFoutbericht(exception, instance);
	}

	private void SetHeaders(HttpContext httpContext)
	{
		// Removed api-version because Haal Centraal chose to add it to the URL.
		//const string versionHeader = "api-version";
		//httpContext.Items.TryGetValue(versionHeader, out var apiVersion);
		//if (apiVersion != null)
		//    httpContext.Response.Headers.TryAdd(versionHeader, (string)apiVersion);

		httpContext.Response.OnStarting((state) =>
		{
			httpContext.Response.ContentType = new MediaTypeHeaderValue("application/problem+json")
			{
				CharSet = "utf-8"
			}.ToString();

			return Task.FromResult(0);
		}, null!);
	}

	private BadRequestFoutbericht CreateBadRequestFoutbericht(Exception exception, string instance)
	{
		BadRequestFoutbericht foutBericht = exception switch
		{
			InvalidRequestBodyException invalidRequestBodyException => CreateBadRequestFoutbericht(invalidRequestBodyException, instance),
			InvalidParamsException invalidParamsException => CreateBadRequestFoutbericht(invalidParamsException, instance),
			_ => CreateBadRequestFoutbericht((InvalidParamsException)exception, instance),
		};
		return foutBericht;
	}

	private BadRequestFoutbericht CreateBadRequestFoutbericht(InvalidParamsException exception, string instance)
	{
		return new BadRequestFoutbericht
		{
			Code = exception.ErrorCode.ToString(),
			Title = exception.Title,
			Detail = exception.Details,
			Status = 400,
			//Type = GetType(400),
			Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
			Instance = instance,
			InvalidParams = exception.InvalidParams?.ToList()
		};
	}

	private BadRequestFoutbericht CreateBadRequestFoutbericht(InvalidRequestBodyException exception, string instance)
	{
		return new BadRequestFoutbericht
		{
			Code = exception.ErrorCode.ToString(),
			Title = exception.Title,
			Detail = exception.Details,
			Status = 400,
			//Type = GetType(400),
			Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
			Instance = instance
		};
	}

	private InvalidParams CreateInvalidParam(ActionContext actionContext, string errorPropertyName, string errorMessage)
	{
		var code = ValidationErrorMessages.GetInvalidParamCode(errorMessage);

		// type is null when the model state contains an unknown validation error. Example: cannot parse "abcde" to integer.
		// GetParseErrorMessage gets the correct Dutch error message (example: Waarde is geen geldige integer.) based on the type of the parameter.
		if (code == null)
		{
			var parseErrorCode = GetParsingErrorCode(actionContext as ActionExecutingContext, errorPropertyName);
			code = parseErrorCode?.ToString().Replace("_", ""); //remove _ for _enum and _string
			errorMessage = ValidationErrorMessages.GetParseErrorMessage(parseErrorCode) ?? errorMessage;
		}
		else if (code.Equals(InvalidParamCode.pattern.ToString()) && !errorMessage.Contains("Waarde voldoet niet aan patroon"))
		{
			errorMessage = errorMessage.Replace($"The field {errorPropertyName} must match the regular expression ", "Waarde voldoet niet aan patroon");
		}

		return new InvalidParams
		{
			Name = errorPropertyName,
			Reason = errorMessage,
			Code = code
		};
	}

	private InvalidParamCode? GetParsingErrorCode(ActionExecutingContext? actionContext, string errorPropertyName)
	{
		if (actionContext == null)
		{
			return null;
		}

		var propInfo = actionContext.ActionArguments.Select(x => x.Value?.GetType().GetProperty(errorPropertyName)).FirstOrDefault(x => x != null);

		if (propInfo == null)
		{
			return null;
		}
		else if (propInfo.PropertyType.IsEnum || Nullable.GetUnderlyingType(propInfo.PropertyType)?.IsEnum == true)
		{
			return InvalidParamCode._enum;
		}

		return GetInvalidParamCode(propInfo.PropertyType);
	}

	/// <summary>
	/// Return invalid param code if given type is known within dictionary.
	/// Also performs a secondary check if type is nullable where its underlying type is known to the dictionary.
	/// Return null when no options are known within the dictionary.
	/// </summary>
	/// <param name="objectType"></param>
	/// <returns></returns>
	private InvalidParamCode? GetInvalidParamCode(Type objectType)
	{
		if (_typeCodeEnumDictionary.ContainsKey(objectType))
		{
			return _typeCodeEnumDictionary[objectType];
		}

		var nullableUnderlyingType = Nullable.GetUnderlyingType(objectType);
		if (nullableUnderlyingType != null && _typeCodeEnumDictionary.ContainsKey(nullableUnderlyingType))
		{
			return _typeCodeEnumDictionary[nullableUnderlyingType];
		}

		return null;
	}


	//private string GetType(int httpStatusCode) => "https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?#System_Net_HttpStatusCode_" + ReasonPhrases.GetReasonPhrase(httpStatusCode).Replace(" ", "")

	// private const string BadRequestIdentifier = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
	// private const string MethodNotAllowedIdentifier = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.5"
	// private const string NotAcceptableIdentifier = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6"
	// private const string UnsupportedMediaTypeIdentifier = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.13"
	// private const string InternalServerErrorIdentifier = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
	/// <summary>
	/// Get URL for error type explanation for error faults.
	/// </summary>
	/// <param name="httpStatusCode"></param>
	/// <returns></returns>
	private string GetType(int httpStatusCode)
	{
		string prefix = "https://datatracker.ietf.org/doc/html/rfc7231#";

		return httpStatusCode switch
		{
			400 => $"{prefix}section-6.5.1",
			401 => $"{prefix.Replace("rfc7231", "rfc7235")}section-3.1",
			403 => $"{prefix}section-6.5.3",
			404 => $"{prefix}section-6.5.4",
			405 => $"{prefix}section-6.5.5",
			406 => $"{prefix}section-6.5.6",
			415 => $"{prefix}section-6.5.13",
			500 => $"{prefix}section-6.6.1",
			501 => $"{prefix}section-6.6.2",
			503 => $"{prefix}section-6.6.4",
			_ => $"{prefix}section-6.6.1"
		};
	}

	private string GetInstance(string path)
	{
		//return $"https://{_haalcentraalApiOptions.Value.BrpHostName}{path}";
		return path;
	}
}