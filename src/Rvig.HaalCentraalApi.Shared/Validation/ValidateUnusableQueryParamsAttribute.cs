using Microsoft.AspNetCore.Mvc.Filters;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public class ValidateUnusableQueryParamsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var queryParams = context.HttpContext.Request.Query.Keys;
        var validParams = context.ActionArguments.Where(x => x.Value != null).SelectMany(x => x.Value!.GetType().GetProperties().Select(x => x.Name));

        var invalidParams = queryParams.Where(x => !validParams.Contains(x)).Select(x => new InvalidParams
        {
            Name = x,
            Code = InvalidParamCode.unknownParam.ToString(),
            Reason = ValidationErrorMessages.UnexpectedParam
        });

        if (invalidParams.Any())
        {
            throw new InvalidParamsException(invalidParams);
        }

        base.OnActionExecuting(context);
    }
}