using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AdOut.Planning.WebApi.Filters
{
    public class ModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {

                var errors = context.ModelState.Values.SelectMany(entry => entry.Errors)
                                                      .Select(error => error.ErrorMessage);
                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
