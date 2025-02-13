using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rvig.Base.App.Services;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;

namespace Rvig.Base.App.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class GlobalErrorsController : Controller
{
	private readonly IErrorResponseService _errorResponseService;
	private readonly ILogger<GlobalErrorsController> _logger;
	private readonly ILoggingHelper _loggingHelper;

	public GlobalErrorsController(IErrorResponseService apiValidationService, ILogger<GlobalErrorsController> logger, ILoggingHelper loggingHelper)
	{
		_errorResponseService = apiValidationService;
		_logger = logger;
		_loggingHelper = loggingHelper;
	}

	[Route("error")]
	public Foutbericht HandleError()
	{
		var exceptionPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

		// prevent directly calling this endpoint by an api user
		if (exceptionPathFeature == null)
		{
			_logger.LogError("{Exception}", new NotFoundException($"Resource {HttpContext.Request.Path} niet gevonden."));
			throw new NotFoundException($"Resource {HttpContext.Request.Path} niet gevonden.");
		}

		if (exceptionPathFeature.Error is InvalidParamsException invalidParamsException)
		{
			var badRequest = _errorResponseService.CreateBadRequestFoutbericht(HttpContext, invalidParamsException, exceptionPathFeature.Path);
			//_loggingHelper.LogError(badRequest);
			return badRequest;
		}

		if (!(exceptionPathFeature.Error is IHaalCentraalException))
		{
			_logger.LogError("WebAPI throwed unhandled exception. {Error}", exceptionPathFeature.Error);
		}

		return _errorResponseService.CreateFoutBericht(HttpContext, Response, exceptionPathFeature.Error, exceptionPathFeature.Path);
	}

	[Route("error/{statuscode}")]
	public Foutbericht ErrorStatuscode(int statuscode)
	{
		var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

		// prevent directly calling this endpoint by an api user
		if (statusCodeReExecuteFeature == null)
		{
			_logger.LogError("{Exception}", new NotFoundException($"Resource {HttpContext.Request.Path} niet gevonden."));
			throw new NotFoundException($"Resource {HttpContext.Request.Path} niet gevonden.");
		}

		Exception exception = statuscode switch
		{
			401 => new AuthenticationException(),
			403 => new AuthorizationException(),
			404 => new NotFoundException($"Resource {statusCodeReExecuteFeature.OriginalPath} niet gevonden."),
			405 => new MethodNotAllowedException(),
			_ => throw CreateDefaultAndLogException($"Reexecuted statuscode not handled: {statuscode}")
		};

		return _errorResponseService.CreateFoutBericht(HttpContext, Response, exception, statusCodeReExecuteFeature.OriginalPath);
	}

	private Exception CreateDefaultAndLogException(string message)
	{
		var exception = new CustomNotImplementedException(message);
		_logger.LogError("{Exception}", exception);
		return new CustomNotImplementedException();
	}
}