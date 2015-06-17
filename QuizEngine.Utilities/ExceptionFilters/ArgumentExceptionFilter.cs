using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuizEngine.Utilities.ExceptionFilters
{
    public class ArgumentExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            if(exception is ArgumentException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}
