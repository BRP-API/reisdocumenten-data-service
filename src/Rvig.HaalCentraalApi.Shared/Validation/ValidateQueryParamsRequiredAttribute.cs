using Microsoft.AspNetCore.Mvc.Filters;
using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public class ValidateQueryParamsRequiredAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Query.Count == 0)
        {
            throw new ParamsRequiredException();
        }

        base.OnActionExecuting(context);
    }
}