using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Data.Model;

namespace MarketPlace.Utilities.Filter
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ErrorModel error = new ErrorModel();
                error.success = false;
                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(v => v.Errors);
                modelErrors.ToList().ForEach(x =>
                {
                    error.message = error.message + "," + x.ErrorMessage;
                });
                error.message = error.message.Substring(1);
                context.Result = new OkObjectResult(error);
            }
        }
    }
}
