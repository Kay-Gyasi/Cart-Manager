using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Linq;

namespace Hubtel.ECommerce.API
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();
                var builder = new StringBuilder();
                foreach (var error in errors)
                {
                    builder.AppendLine(error);
                }

                var responseObj = new
                {
                    Errors = errors.ToString()
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
