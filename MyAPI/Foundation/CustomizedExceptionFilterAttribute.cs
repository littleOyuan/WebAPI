using MyAPI.Base;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace MyAPI.Foundation
{
    public class CustomizedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ResponseBase>(new ResponseBase(false, false, actionExecutedContext.Exception.Message), new JsonMediaTypeFormatter(), "application/json")
            };
        }
    }
}